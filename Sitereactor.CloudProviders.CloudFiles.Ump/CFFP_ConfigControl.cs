using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TheOutfield.UmbExt.UniversalMediaPicker.Controls;
using TheOutfield.UmbExt.UniversalMediaPicker.Extensions;

namespace Sitereactor.CloudProviders.CloudFiles.Ump
{
    public class CFFP_ConfigControl : AbstractConfigControl
    {
        private CFFP_Provider _provider;
        private readonly CFFP_Config _config;
        private readonly TextBox _apiKeyTextBox = new TextBox();
        private readonly TextBox _usernameTextBox = new TextBox();

        public CFFP_ConfigControl(CFFP_Provider provider, string options)
        {
            this._provider = provider;
            this._config = options.DeserializeJsonTo<CFFP_Config>();
        }

        #region Overrides of AbstractConfigControl
        public override string Value
        {
            get
            {
                EnsureChildControls();

                _config.ApiKey = _apiKeyTextBox.Text;
                _config.Username = _usernameTextBox.Text;

                return _config.SerializeToJson();
            }
        }
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                _apiKeyTextBox.Text = _config.ApiKey;
                _usernameTextBox.Text = _config.Username;
            }

            base.OnLoad(e);
        }

        protected override void CreateChildControls()
        {
            _apiKeyTextBox.ID = "apiKeyTextBox";
            _usernameTextBox.ID = "secretKeyTextBox";

            _apiKeyTextBox.CssClass = "guiInputText guiInputStandardSize";
            _usernameTextBox.CssClass = "guiInputText guiInputStandardSize";

            Controls.Add(_apiKeyTextBox);
            Controls.Add(_usernameTextBox);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddFormRow("API Key:", "Please enter your Cloud Files API key", _apiKeyTextBox);
            writer.AddFormRow("Username:", "Please enter your Cloud Files Username", _usernameTextBox);
        }
    }
}
