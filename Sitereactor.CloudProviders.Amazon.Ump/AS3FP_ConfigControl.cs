using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ListBox _bucketsListBox = new ListBox();

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
                _config.AllowedBuckets = _bucketsListBox.Items.Cast<ListItem>()
                    .Where(item => item.Selected)
                    .Select(item => item.Value)
                    .ToList();

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

                if(!string.IsNullOrEmpty(_config.AccessKey) && !string.IsNullOrEmpty(_config.SecretKey))
                {
                    var factory = new StorageFactory(_config.AccessKey, _config.SecretKey);
                    var buckets = factory.GetAllBuckets();

                    if (_config.AllowedBuckets == null)
                    {
                        _config.AllowedBuckets = new List<string>();
                    }

                    foreach (var bucket in buckets)
                    {
                        var item = new ListItem(bucket, bucket)
                                       {
                                           Selected = _config.AllowedBuckets.Contains(bucket)
                                       };

                        _bucketsListBox.Items.Add(item);
                    }
                }
            }

            base.OnLoad(e);
        }

        protected override void CreateChildControls()
        {
            _accessKeyTextBox.ID = "apiKeyTextBox";
            _secretKeyTextBox.ID = "secretKeyTextBox";
            _cdnDomainTextBox.ID = "cdnDomainTextBox";
            _bucketsListBox.ID = "bucketsListBox";

            _accessKeyTextBox.CssClass = "guiInputText guiInputStandardSize";
            _secretKeyTextBox.CssClass = "guiInputText guiInputStandardSize";
            _cdnDomainTextBox.CssClass = "guiInputText guiInputStandardSize";

            _bucketsListBox.SelectionMode = ListSelectionMode.Multiple;

            Controls.Add(_accessKeyTextBox);
            Controls.Add(_secretKeyTextBox);
            Controls.Add(_cdnDomainTextBox);
            Controls.Add(_bucketsListBox);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //TODO: Add dropdown list to select bucket, which Cloud Front URL is mapped to
            writer.AddFormRow("Access Key:", "Please enter your Amazon AWS Access key", _accessKeyTextBox);
            writer.AddFormRow("Secret Key:", "Please enter your Amazon AWS Secret Key", _secretKeyTextBox);
            writer.AddFormRow("Custom CDN Domain:", "Please enter Cloud Front domain mapped to your S3 storage", _cdnDomainTextBox);
            writer.AddFormRow("Allowed Buckets:", "Select the buckets you want to use", _bucketsListBox);
        }
    }
}
