using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TheOutfield.UmbExt.UniversalMediaPicker.Controls;
using TheOutfield.UmbExt.UniversalMediaPicker.Extensions;

namespace Sitereactor.CloudProviders.Amazon.Ump
{
    public class AS3FP_ConfigControl : AbstractConfigControl
    {
        private AS3FP_Provider _provider;
        private readonly AS3FP_Config _config;
        private readonly TextBox _accessKeyTextBox = new TextBox();
        private readonly TextBox _secretKeyTextBox = new TextBox();
        private readonly TextBox _cdnDomainTextBox = new TextBox();

        public AS3FP_ConfigControl(AS3FP_Provider provider, string options)
        {
            this._provider = provider;
            this._config = options.DeserializeJsonTo<AS3FP_Config>();
        }

        #region Overrides of AbstractConfigControl
        public override string Value
        {
            get
            {
                EnsureChildControls();

                _config.AccessKey = _accessKeyTextBox.Text;
                _config.SecretKey = _secretKeyTextBox.Text;
                _config.CdnDomain = _cdnDomainTextBox.Text;

                return _config.SerializeToJson();
            }
        }
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                _accessKeyTextBox.Text = _config.AccessKey;
                _secretKeyTextBox.Text = _config.SecretKey;
                _cdnDomainTextBox.Text = _config.CdnDomain;
            }

            base.OnLoad(e);
        }

        protected override void CreateChildControls()
        {
            _accessKeyTextBox.ID = "apiKeyTextBox";
            _secretKeyTextBox.ID = "secretKeyTextBox";
            _cdnDomainTextBox.ID = "cdnDomainTextBox";

            _accessKeyTextBox.CssClass = "guiInputText guiInputStandardSize";
            _secretKeyTextBox.CssClass = "guiInputText guiInputStandardSize";
            _cdnDomainTextBox.CssClass = "guiInputText guiInputStandardSize";

            Controls.Add(_accessKeyTextBox);
            Controls.Add(_secretKeyTextBox);
            Controls.Add(_cdnDomainTextBox);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //TODO: Add dropdown list to select bucket, which Cloud Front URL is mapped to
            writer.AddFormRow("Access Key:", "Please enter your Amazon AWS Access key", _accessKeyTextBox);
            writer.AddFormRow("Secret Key:", "Please enter your Amazon AWS Secret Key", _secretKeyTextBox);
            writer.AddFormRow("Custom CDN Domain:", "Please enter Cloud Front domain mapped to your S3 storage", _cdnDomainTextBox);
        }
    }
}
