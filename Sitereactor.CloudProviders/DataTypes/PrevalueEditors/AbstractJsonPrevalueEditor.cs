﻿using System;
using System.Web.Script.Serialization;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.datatype;

namespace Sitereactor.CloudProviders.DataTypes.PrevalueEditors
{
    /// <summary>
    /// Abstract class for the PreValue Editor.
    /// Specifically designed to serialize/deserialize the options as JSON.
    /// </summary>
    public abstract class AbstractJsonPrevalueEditor : AbstractPrevalueEditor
    {
        /// <summary>
        /// The underlying base data-type.
        /// </summary>
        private readonly BaseDataType m_DataType;

        /// <summary>
        /// An object to temporarily lock writing to the database.
        /// </summary>
        private static readonly object m_Locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractJsonPrevalueEditor"/> class.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="dbType">Type of the database field.</param>
        public AbstractJsonPrevalueEditor(BaseDataType dataType, DBTypes dbType)
            : base()
        {
            this.m_DataType = dataType;
            this.m_DataType.DBType = dbType;
        }

        /// <summary>
        /// Gets the PreValue options for the data-type.
        /// </summary>
        /// <typeparam name="T">The type of the resulting object.</typeparam>
        /// <returns>
        /// Returns the options for the PreValue Editor
        /// </returns>
        public T GetPreValueOptions<T>()
        {
            var prevalues = PreValues.GetPreValues(this.m_DataType.DataTypeDefinitionId);
            if (prevalues.Count > 0)
            {
                var prevalue = (PreValue)prevalues[0];
                if (!String.IsNullOrEmpty(prevalue.Value))
                {
                    try
                    {
                        // deserialize the options
                        var serializer = new JavaScriptSerializer();

                        // return the options
                        return serializer.Deserialize<T>(prevalue.Value);
                    }
                    catch (Exception ex)
                    {
                        Log.Add(LogTypes.Error, this.m_DataType.DataTypeDefinitionId, string.Concat("uCloud: Execption thrown: ", ex.Message));
                    }
                }
            }

            // if all else fails, return default options
            return default(T);
        }

        /// <summary>
        /// Saves the data-type PreValue options.
        /// </summary>
        public void SaveAsJson(object options)
        {
            // serialize the options into JSON
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(options);

            lock (m_Locker)
            {
                var prevalues = PreValues.GetPreValues(this.m_DataType.DataTypeDefinitionId);
                if (prevalues.Count > 0)
                {
                    PreValue prevalue = (PreValue)prevalues[0];

                    // update
                    prevalue.Value = json;
                    prevalue.Save();
                }
                else
                {
                    // insert
                    PreValue.MakeNew(this.m_DataType.DataTypeDefinitionId, json);
                }
            }
        }
    }
}
