using Sitereactor.CloudProviders.DataTypes.PrevalueEditors;

namespace Sitereactor.CloudProviders.Amazon.DataTypes.AmazonS3Uploader
{
    public class AS3U_Options : AbstractOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AS3U_Options"/> class.
        /// </summary>
        public AS3U_Options()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AS3U_Options"/> class.
        /// </summary>
        /// <param name="loadDefaults">if set to <c>true</c> [load defaults].</param>
        public AS3U_Options(bool loadDefaults)
            : base(loadDefaults)
        {
        }

        /// <summary>
        /// Gets or sets the account name for authentication.
        /// </summary>
        /// <value>The account name for authentication.</value>
        public string AccessKey { get; set; }

        /// <summary>
        /// Gets or sets the key for authentication.
        /// </summary>
        /// <value>The key for authentication.</value>
        public string SecretKey { get; set; }
    }
}
