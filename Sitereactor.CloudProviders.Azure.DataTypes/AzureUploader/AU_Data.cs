using System.Xml;
using umbraco.cms.businesslogic.datatype;

namespace Sitereactor.CloudProviders.Azure.DataTypes.AzureUploader
{
    /// <summary>
    /// Overrides the <see cref="umbraco.cms.businesslogic.datatype.DefaultData"/> object to return the value as XML.
    /// </summary>
    public class AU_Data : DefaultData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlData"/> class.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        public AU_Data(BaseDataType dataType)
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
                string result = this.Value.ToString();
                // return the value
                return data.CreateTextNode(result);
            }
            else
            {
                // otherwise render the value as default (in CDATA)
                return base.ToXMl(data);
            }
        }
    }
}
