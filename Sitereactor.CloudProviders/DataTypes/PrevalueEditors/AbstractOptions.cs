using System.ComponentModel;

namespace Sitereactor.CloudProviders.DataTypes.PrevalueEditors
{
    /// <summary>
    /// Abstract class for the Prevalue options.
    /// </summary>
    public abstract class AbstractOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractOptions"/> class.
        /// </summary>
        public AbstractOptions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractOptions"/> class.
        /// </summary>
        /// <param name="loadDefaults">if set to <c>true</c> [load defaults].</param>
        public AbstractOptions(bool loadDefaults)
        {
            if (loadDefaults)
            {
                var type = this.GetType();

                foreach (var property in type.GetProperties())
                {
                    foreach (DefaultValueAttribute attribute in property.GetCustomAttributes(typeof(DefaultValueAttribute), true))
                    {
                        property.SetValue(this, attribute.Value, null);
                    }
                }
            }
        }
    }
}
