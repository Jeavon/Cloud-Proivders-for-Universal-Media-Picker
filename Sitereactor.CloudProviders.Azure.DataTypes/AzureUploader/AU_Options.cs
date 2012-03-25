using Sitereactor.CloudProviders.DataTypes.PrevalueEditors;

namespace Sitereactor.CloudProviders.Azure.DataTypes.AzureUploader
{
    public class AU_Options : AbstractOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AU_Options"/> class.
        /// </summary>
        public AU_Options()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AU_Options"/> class.
        /// </summary>
        /// <param name="loadDefaults">if set to <c>true</c> [load defaults].</param>
        public AU_Options(bool loadDefaults)
            : base(loadDefaults)
        {
        }

        /// <summary>
        /// Gets or sets the account name for authentication.
        /// </summary>
        /// <value>The account name for authentication.</value>
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the key for authentication.
        /// </summary>
        /// <value>The key for authentication.</value>
        public string Key { get; set; }
    }
}
