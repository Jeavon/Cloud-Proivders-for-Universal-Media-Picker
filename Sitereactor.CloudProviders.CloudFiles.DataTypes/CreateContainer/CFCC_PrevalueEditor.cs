using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitereactor.CloudProviders.DataTypes.PrevalueEditors;
using Sitereactor.CloudProviders.Extensions;
using umbraco.cms.businesslogic.datatype;

namespace Sitereactor.CloudProviders.CloudFiles.DataTypes.CreateContainer
{
    public class CFCC_PrevalueEditor : AbstractJsonPrevalueEditor
    {
        /// <summary>
        /// TextBox control for the account username.
        /// </summary>
        private TextBox txtUsername;

        /// <summary>
        /// TextBox control for the account api key.
        /// </summary>
        private TextBox txtApiKey;

        /// <summary>
        /// CheckBox control for meta data
        /// </summary>
        private CheckBox chkShowMetaData;

        /// <summary>
        /// DataType Options
        /// </summary>
        private CFCC_Options cfcc_options;

        /// <summary>
        /// Initializes a new instance of the <see cref="CFCC_PrevalueEditor"/> class.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        public CFCC_PrevalueEditor(BaseDataType dataType)
            : base(dataType, DBTypes.Ntext)
        {
            // get PreValues, load them into the controls.
            cfcc_options = this.GetPreValueOptions<CFCC_Options>();
        }

        /// <summary>
        /// Saves the data-type PreValue options.
        /// </summary>
        public override void Save()
        {
            // set the options
            var options = new CFCC_Options()
            {
                Username = this.txtUsername.Text,
                ApiKey = this.txtApiKey.Text,
                ShowMetaData = this.chkShowMetaData.Checked
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
            this.txtUsername = new TextBox() { ID = "txtUsername", CssClass = "guiInputText umbEditorTextField" };
            this.txtApiKey = new TextBox() { ID = "txtApiKey", CssClass = "guiInputText umbEditorTextField" };
            this.chkShowMetaData = new CheckBox() { ID = "chkShowMetaData" };

            // add the child controls
            this.Controls.AddPrevalueControls(this.txtUsername, this.txtApiKey, this.chkShowMetaData);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // if the options are null, then load the defaults
            if (cfcc_options == null)
            {
                cfcc_options = new CFCC_Options(true);
            }

            // set the values
            this.txtUsername.Text = cfcc_options.Username;
            this.txtApiKey.Text = cfcc_options.ApiKey;
            this.chkShowMetaData.Checked = cfcc_options.ShowMetaData;
        }

        /// <summary>
        /// Renders the contents of the control to the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            // add property fields
            writer.AddPrevalueHeading("Cloud Files API Settings", "Add your Rackspace Cloud Files API token and username here");
            writer.AddPrevalueRow("Username:", this.txtUsername);
            writer.AddPrevalueRow("API Token:", this.txtApiKey);
            writer.AddPrevalueRow("Show Meta Data:", this.chkShowMetaData);
        }
    }
}