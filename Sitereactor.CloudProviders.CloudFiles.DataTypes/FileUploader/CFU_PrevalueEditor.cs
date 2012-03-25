using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitereactor.CloudProviders.Extensions;
using umbraco.cms.businesslogic.datatype;
using Sitereactor.CloudProviders.DataTypes.PrevalueEditors;

namespace Sitereactor.CloudProviders.CloudFiles.DataTypes.FileUploader
{
    public class CFU_PrevalueEditor : AbstractJsonPrevalueEditor
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
        /// DropDownList for selecting a default container.
        /// </summary>
        private DropDownList ddlDefaultContainer;

        /// <summary>
        /// TextBox control for the alias of the default Container property
        /// </summary>
        private TextBox txtDefaultAlias;

        /// <summary>
        /// DataType Options
        /// </summary>
        private CFU_Options cfu_options;

        /// <summary>
        /// Initializes a new instance of the <see cref="CFU_PrevalueEditor"/> class.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        public CFU_PrevalueEditor(BaseDataType dataType)
            : base(dataType, DBTypes.Ntext)
        {
            // get PreValues, load them into the controls.
            cfu_options = this.GetPreValueOptions<CFU_Options>();
        }

        /// <summary>
        /// Saves the data-type PreValue options.
        /// </summary>
        public override void Save()
        {
            // set the options
            var options = new CFU_Options()
            {
                Username = this.txtUsername.Text,
                ApiKey = this.txtApiKey.Text,
                ShowMetaData = this.chkShowMetaData.Checked,
                DefaultContainer = ddlDefaultContainer.SelectedValue,
                DefaultContainerAlias = txtDefaultAlias.Text
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
            this.txtDefaultAlias = new TextBox() { ID = "txtDefaultAlias", CssClass = "guiInputText umbEditorTextField" };
            this.ddlDefaultContainer = new DropDownList() { ID = "ddlDefaultContainer" };

            this.ddlDefaultContainer.Items.Clear();
            this.ddlDefaultContainer.Items.Add(string.Empty);
            if (!string.IsNullOrEmpty(cfu_options.Username) && !string.IsNullOrEmpty(cfu_options.ApiKey))
            {
                Factory factory = new Factory(cfu_options.Username, cfu_options.ApiKey);
                var containers = factory.GetAllContainersAsSimpleList();
                foreach (string container in containers)
                {
                    this.ddlDefaultContainer.Items.Add(new ListItem(container, container));
                }
            }

            // add the child controls
            this.Controls.AddPrevalueControls(this.txtUsername, this.txtApiKey, this.chkShowMetaData, this.txtDefaultAlias, this.ddlDefaultContainer);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // if the options are null, then load the defaults
            if (cfu_options == null)
            {
                cfu_options = new CFU_Options(true);
            }

            // set the values
            this.txtUsername.Text = cfu_options.Username;
            this.txtApiKey.Text = cfu_options.ApiKey;
            this.chkShowMetaData.Checked = cfu_options.ShowMetaData;
            this.txtDefaultAlias.Text = cfu_options.DefaultContainerAlias;
            this.ddlDefaultContainer.SelectedValue = cfu_options.DefaultContainer;
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
            writer.AddPrevalueRow("Show File Meta Data:", this.chkShowMetaData);
            writer.AddPrevalueHeading("Container Settings", "Enter the property alias of the container datatype or select your default container from the dropdown");
            writer.AddPrevalueRow("Default Container Property Alias:", this.txtDefaultAlias);
            writer.AddPrevalueRow("Default Container:", this.ddlDefaultContainer);
        }
    }
}
