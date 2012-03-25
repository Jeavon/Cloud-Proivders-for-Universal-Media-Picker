using Sitereactor.CloudProviders.DataTypes.PrevalueEditors;

namespace Sitereactor.CloudProviders.CloudFiles.DataTypes.CreateContainer
{
    public class CFCC_Options : AbstractOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CFCC_Options"/> class.
        /// </summary>
        public CFCC_Options()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CFCC_Options"/> class.
        /// </summary>
        /// <param name="loadDefaults">if set to <c>true</c> [load defaults].</param>
        public CFCC_Options(bool loadDefaults)
            : base(loadDefaults)
        {
        }

        /// <summary>
        /// Gets or sets the username for authentication.
        /// </summary>
        /// <value>The username for authentication.</value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the api key for authentication.
        /// </summary>
        /// <value>The api key for authentication.</value>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets a boolean indicating whether to show or hide meta data for a container
        /// </summary>
        public bool ShowMetaData { get; set; }
    }
}
