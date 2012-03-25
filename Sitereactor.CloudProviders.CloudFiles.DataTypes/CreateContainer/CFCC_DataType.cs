using System;
using System.Collections.Generic;
using Sitereactor.CloudProviders.Extensions;
using umbraco.cms.businesslogic.datatype;
using umbraco.interfaces;

namespace Sitereactor.CloudProviders.CloudFiles.DataTypes.CreateContainer
{
    public class CFCC_DataType : AbstractDataEditor
    {
        /// <summary>
        /// The render control.
        /// </summary>
        private CFCC_Control m_Control = new CFCC_Control();

        /// <summary>
        /// The Data object for the data-type.
        /// </summary>
        private IData m_Data;

        /// <summary>
        /// The PreValue Editor for the data-type.
        /// </summary>
        private CFCC_PrevalueEditor m_PreValueEditor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CFCC_DataType"/> class.
        /// </summary>
        public CFCC_DataType()
            : base()
        {
            // set the render control as the placeholder
            this.RenderControl = this.m_Control;

            // assign the initialise event for the placeholder
            this.m_Control.Init += new EventHandler(this.m_Control_Init);

            // assign the save event for the data-type/editor
            this.DataEditorControl.OnSave += new AbstractDataEditorControl.SaveEventHandler(this.DataEditorControl_OnSave);
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>The data.</value>
        public override IData Data
        {
            get
            {
                if (this.m_Data == null)
                {
                    this.m_Data = new CFCC_Data(this);
                }

                return this.m_Data;
            }
        }

        /// <summary>
        /// Gets the name of the data type.
        /// </summary>
        /// <value>The name of the data type.</value>
        public override string DataTypeName
        {
            get
            {
                return "uCloud CDN Providers: Rackspace Cloud Files - Create Container";
            }
        }

        /// <summary>
        /// Gets the id of the data-type.
        /// </summary>
        /// <value>The id of the data-type.</value>
        public override Guid Id
        {
            get
            {
                return new Guid("5538a113-663d-43a3-b4f2-97261222e9db");
            }
        }

        /// <summary>
        /// Gets the prevalue editor.
        /// </summary>
        /// <value>The prevalue editor.</value>
        public override IDataPrevalue PrevalueEditor
        {
            get
            {
                if (this.m_PreValueEditor == null)
                {
                    this.m_PreValueEditor = new CFCC_PrevalueEditor(this);
                }

                return this.m_PreValueEditor;
            }
        }

        /// <summary>
        /// Handles the Init event of the m_Placeholder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void m_Control_Init(object sender, EventArgs e)
        {
            // get the render control options from the Prevalue Editor.
            var options = ((CFCC_PrevalueEditor)this.PrevalueEditor).GetPreValueOptions<CFCC_Options>();

            if (this.Data.Value != null)
            {
                this.m_Control.ControlID = this.DataEditor.Editor.ID.AddDefaultAlias();
                
                var dictionary = this.Data.Value.ToString().DeserializeJsonTo<Dictionary<string, string>>();
                if (dictionary != null)
                {
                    this.m_Control.ContainerName = dictionary.ContainsKey("ContainerName") ? dictionary["ContainerName"] : string.Empty;
                    this.m_Control.IsPublic = dictionary.ContainsKey("Public") ? bool.Parse(dictionary["Public"]) : false;
                }
            }

            // set the render control options
            this.m_Control.Options = options;
        }

        /// <summary>
        /// Saves the editor control value.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void DataEditorControl_OnSave(EventArgs e)
        {
            Factory factory = new Factory(m_Control.Options.Username, m_Control.Options.ApiKey);
            var simpleList = factory.GetAllContainersAsSimpleList();
            if (simpleList.Contains(m_Control.ContainerName))
            {
                if (m_Control.IsPublic)
                {
                    factory.SetPublicContainer(m_Control.ContainerName);
                }
                else
                {
                    factory.SetPrivateContainer(m_Control.ContainerName);
                }
            }
            else
            {
                factory.CreateContainer(m_Control.ContainerName);
                if (m_Control.IsPublic)
                {
                    factory.SetPublicContainer(m_Control.ContainerName);
                }
            }

            var dictionary = new Dictionary<string, string>
                                 {
                                     {"ContainerName", m_Control.ContainerName},
                                     {"Public", m_Control.IsPublic.ToString()}
                                 };

            // save the value of the control depending on whether a new file is uploaded);
            this.Data.Value = dictionary.SerializeToJson();
        }
    }
}
