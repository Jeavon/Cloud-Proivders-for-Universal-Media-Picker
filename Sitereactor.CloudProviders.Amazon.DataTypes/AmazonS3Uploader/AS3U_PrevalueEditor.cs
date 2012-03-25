using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitereactor.CloudProviders.Extensions;
using Sitereactor.CloudProviders.DataTypes.PrevalueEditors;
using umbraco.cms.businesslogic.datatype;

namespace Sitereactor.CloudProviders.Amazon.DataTypes.AmazonS3Uploader
{
    public class AS3U_PrevalueEditor : AbstractJsonPrevalueEditor
    {
        /// <summary>
        /// TextBox control for the account username.
        /// </summary>
        private TextBox txtAccessKey;

        /// <summary>
        /// TextBox control for the account api key.
        /// </summary>
        private TextBox txtSecretKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="AS3U_PrevalueEditor"/> class.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        public AS3U_PrevalueEditor(BaseDataType dataType)
            : base(dataType, DBTypes.Ntext)
        {
        }

        /// <summary>
        /// Saves the data-type PreValue options.
        /// </summary>
        public override void Save()
        {
            // set the options
            var options = new AS3U_Options()
            {
                AccessKey = this.txtAccessKey.Text,
                SecretKey = this.txtSecretKey.Text
            };

            // save the options as JSON
            this.SaveAsJson(options);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.EnsureChildControls();
        }

        /// <summary>
        /// Creates child controls for this control
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            // set-up child controls
            this.txtAccessKey = new TextBox() { ID = "txtAccessKey", CssClass = "guiInputText umbEditorTextField" };
            this.txtSecretKey = new TextBox() { ID = "txtSecretKey", CssClass = "guiInputText umbEditorTextField" };

            // add the child controls
            this.Controls.AddPrevalueControls(this.txtAccessKey, this.txtSecretKey);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // get PreValues, load them into the controls.
            var options = this.GetPreValueOptions<AS3U_Options>();

            // if the options are null, then load the defaults
            if (options == null)
            {
                options = new AS3U_Options(true);
            }

            // set the values
            this.txtAccessKey.Text = options.AccessKey;
            this.txtSecretKey.Text = options.SecretKey;
        }

        /// <summary>
        /// Renders the contents of the control to the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            // add property fields
            writer.AddPrevalueHeading("Amazon S3 and Cloud Front API Settings");
            writer.AddPrevalueRow("", "Add your Amazon account information here.");
            writer.AddPrevalueRow("Access Key:", this.txtAccessKey);
            writer.AddPrevalueRow("Secret Key:", this.txtSecretKey);
        }
    }
}
