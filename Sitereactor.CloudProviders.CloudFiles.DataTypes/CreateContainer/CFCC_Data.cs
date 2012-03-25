using System.Collections.Generic;
using System.Xml;
using Sitereactor.CloudProviders.Extensions;
using umbraco.cms.businesslogic.datatype;

namespace Sitereactor.CloudProviders.CloudFiles.DataTypes.CreateContainer
{
    /// <summary>
    /// Overrides the <see cref="umbraco.cms.businesslogic.datatype.DefaultData"/> object to return the value as XML.
    /// </summary>
    public class CFCC_Data : DefaultData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlData"/> class.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        public CFCC_Data(BaseDataType dataType)
            : base(dataType)
        {
        }

        /// <summary>
        /// Converts the data value to XML.
        /// </summary>
        /// <param name="data">The data to convert to XML.</param>
        /// <returns></returns>
        public override XmlNode ToXMl(XmlDocument data)
        {
            if (this.Value != null && !string.IsNullOrEmpty(this.Value.ToString()))
            {
                var dictionary = this.Value.ToString().DeserializeJsonTo<Dictionary<string, string>>();
                string containerName = dictionary.ContainsKey("ContainerName") ? dictionary["ContainerName"] : string.Empty;
                
                // return the value
                return data.CreateTextNode(containerName);
            }
            else
            {
                // otherwise render the value as default (in CDATA)
                return base.ToXMl(data);
            }
        }
    }
}
