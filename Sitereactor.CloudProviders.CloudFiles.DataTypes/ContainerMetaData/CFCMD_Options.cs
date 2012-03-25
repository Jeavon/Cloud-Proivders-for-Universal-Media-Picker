using Sitereactor.CloudProviders.DataTypes.PrevalueEditors;

namespace Sitereactor.CloudProviders.CloudFiles.DataTypes.ContainerMetaData
{
    public class CFCMD_Options : AbstractOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CFCMD_Options"/> class.
        /// </summary>
        public CFCMD_Options()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CFCMD_Options"/> class.
        /// </summary>
        /// <param name="loadDefaults">if set to <c>true</c> [load defaults].</param>
        public CFCMD_Options(bool loadDefaults)
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
        /// Gets or sets the default container.
        /// </summary>
        /// <value>The default cloud file container.</value>
        public string DefaultContainer { get; set; }
    }
}
