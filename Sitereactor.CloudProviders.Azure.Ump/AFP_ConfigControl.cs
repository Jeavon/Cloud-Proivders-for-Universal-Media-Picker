using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TheOutfield.UmbExt.UniversalMediaPicker.Controls;
using TheOutfield.UmbExt.UniversalMediaPicker.Extensions;

namespace Sitereactor.CloudProviders.Azure.Ump
{
    public class AFP_ConfigControl : AbstractConfigControl
    {
        private AFP_Provider _provider;
        private readonly AFP_Config _config;
        private readonly TextBox _accountNameTextBox = new TextBox();
        private readonly TextBox _keyTextBox = new TextBox();
        private readonly TextBox _cdnDomainTextBox = new TextBox();

        public AFP_ConfigControl(AFP_Provider provider, string options)
        {
            this._provider = provider;
            this._config = options.DeserializeJsonTo<AFP_Config>();
        }

        #region Overrides of AbstractConfigControl
        public override string Value
        {
            get
            {
                EnsureChildControls();

                _config.AccountName = _accountNameTextBox.Text;
                _config.Key = _keyTextBox.Text;
                _config.CdnDomain = _cdnDomainTextBox.Text;

                return _config.SerializeToJson();
            }
        }
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                _accountNameTextBox.Text = _config.AccountName;
                _keyTextBox.Text = _config.Key;
                _cdnDomainTextBox.Text = _config.CdnDomain;
            }

            base.OnLoad(e);
        }

        protected override void CreateChildControls()
        {
            _accountNameTextBox.ID = "accountNameTextBox";
            _keyTextBox.ID = "keyTextBox";
            _cdnDomainTextBox.ID = "cdnDomainTextBox";

            _accountNameTextBox.CssClass = "guiInputText guiInputStandardSize";
            _keyTextBox.CssClass = "guiInputText guiInputStandardSize";
            _cdnDomainTextBox.CssClass = "guiInputText guiInputStandardSize";

            Controls.Add(_accountNameTextBox);
            Controls.Add(_keyTextBox);
            Controls.Add(_cdnDomainTextBox);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddFormRow("Account Name:", "Please enter your Azure Account Name", _accountNameTextBox);
            writer.AddFormRow("Key:", "Please enter your Azure Primary Key", _keyTextBox);
            writer.AddFormRow("Custom CDN Domain:", "Please enter domain mapped to your blob storage", _cdnDomainTextBox);
        }
    }
}
