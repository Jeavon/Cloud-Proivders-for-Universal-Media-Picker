using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitereactor.CloudProviders.Extensions;
using umbraco.cms.businesslogic.datatype;
using Sitereactor.CloudProviders.DataTypes.PrevalueEditors;

namespace Sitereactor.CloudProviders.Azure.DataTypes.AzureUploader
{
    public class AU_PrevalueEditor : AbstractJsonPrevalueEditor
    {
        /// <summary>
        /// TextBox control for the account username.
        /// </summary>
        private TextBox txtAccountName;

        /// <summary>
        /// TextBox control for the account api key.
        /// </summary>
        private TextBox txtKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="AU_PrevalueEditor"/> class.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        public AU_PrevalueEditor(BaseDataType dataType)
            : base(dataType, DBTypes.Ntext)
        {
        }

        /// <summary>
        /// Saves the data-type PreValue options.
        /// </summary>
        public override void Save()
        {
            // set the options
            var options = new AU_Options()
            {
                AccountName = this.txtAccountName.Text,
                Key = this.txtKey.Text
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
            this.txtAccountName = new TextBox() { ID = "txtAccountName", CssClass = "guiInputText umbEditorTextField" };
            this.txtKey = new TextBox() { ID = "txtKey", CssClass = "guiInputText umbEditorTextField" };

            // add the child controls
            this.Controls.AddPrevalueControls(this.txtAccountName, this.txtKey);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // get PreValues, load them into the controls.
            var options = this.GetPreValueOptions<AU_Options>();

            // if the options are null, then load the defaults
            if (options == null)
            {
                options = new AU_Options(true);
            }

            // set the values
            this.txtAccountName.Text = options.AccountName;
            this.txtKey.Text = options.Key;
        }

        /// <summary>
        /// Renders the contents of the control to the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            // add property fields
            writer.AddPrevalueHeading("Azure CDN API Settings");
            writer.AddPrevalueRow("", "Add your Azure account information here.");
            writer.AddPrevalueRow("Account Name:", this.txtAccountName);
            writer.AddPrevalueRow("Key:", this.txtKey);
        }
    }
}
