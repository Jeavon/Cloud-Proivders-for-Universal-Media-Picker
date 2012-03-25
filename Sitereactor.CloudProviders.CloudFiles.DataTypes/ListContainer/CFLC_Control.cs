using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rackspace.Cloudfiles;
using Sitereactor.CloudProviders.Extensions;

namespace Sitereactor.CloudProviders.CloudFiles.DataTypes.ListContainer
{
    public class CFLC_Control : PlaceHolder
    {
        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>The options for the control.</value>
        public CFLC_Options Options { get; set; }

        protected DropDownList ContainerDropdownBox = new DropDownList();
        protected Literal MetaDataLiteral = new Literal();

        private Factory _factory;
        private string _selectedContainer;

        public string SelectedContainer
        {
            get { return ContainerDropdownBox.SelectedValue; }
            set { _selectedContainer = value; }
        }

        public string ControlID { get; set; }

        /// <summary>
        /// Initialize the control, make sure children are created
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.EnsureChildControls();
        }

        /// <summary>
        /// Add the resources (sytles/scripts)
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.ContainerDropdownBox.ID = ControlID;
            ContainerDropdownBox.Items.Add(new ListItem("None", string.Empty));
            if (!string.IsNullOrEmpty(Options.Username) && !string.IsNullOrEmpty(Options.ApiKey))
            {
                //Populate dropdownlist
                ContainerDropdownBox.Items.AddRange(GetContainers());
            }
            //select the already selected container
            if (!string.IsNullOrEmpty(_selectedContainer))
                this.ContainerDropdownBox.SelectedValue = _selectedContainer;

            this.MetaDataLiteral.ID = this.MetaDataLiteral.ClientID;
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.EnsureChildControls();

            this.Controls.Add(this.ContainerDropdownBox);
            this.Controls.Add(this.MetaDataLiteral);
        }

        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the server control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            this.ContainerDropdownBox.RenderControl(writer);

            if (!string.IsNullOrEmpty(ContainerDropdownBox.SelectedValue) && Options.ShowMetaData)
                this.MetaDataLiteral.Text = PopulateWithMetaData(ContainerDropdownBox.SelectedValue);

            this.MetaDataLiteral.RenderControl(writer);
        }

        private string PopulateWithMetaData(string containerName)
        {
            var container = GetContainerByName(containerName);
            StringBuilder sb = new StringBuilder();

            sb.Append("</span>");
            sb.Append("</div>");//close propertyItemContent
            sb.Append("</div>");//close propertyItem

            sb.Append("<div class=\"propertyItem\">");
            sb.Append("<div class=\"propertyItemheader\">TTL (time to live)</div>");
            sb.Append("<div class=\"propertyItemContent\">");
            sb.AppendFormat("{0} (Hours)", container.TTL.ToHours());
            sb.Append("</div>");//close propertyItemContent
            sb.Append("</div>");//close propertyItem

            sb.Append("<div class=\"propertyItem\">");
            sb.Append("<div class=\"propertyItemheader\">Byte Count</div>");
            sb.Append("<div class=\"propertyItemContent\">");
            sb.AppendFormat("{0}", container.BytesUsed);
            sb.Append("</div>");//close propertyItemContent
            sb.Append("</div>");//close propertyItem

            sb.Append("<div class=\"propertyItem\">");
            sb.Append("<div class=\"propertyItemheader\">Object Count</div>");
            sb.Append("<div class=\"propertyItemContent\">");
            sb.AppendFormat("{0}", container.ObjectCount);
            sb.Append("</div>");//close propertyItemContent
            sb.Append("</div>");//close propertyItem

            /*if (!string.IsNullOrEmpty(container.ReferrerACL))
            {
                sb.Append("<div class=\"propertyItem\">");
                sb.Append("<div class=\"propertyItemheader\">Referrer ACL</div>");
                sb.Append("<div class=\"propertyItemContent\">");
                sb.AppendFormat("{0}", container.ReferrerACL);
                sb.Append("</div>");//close propertyItemContent
                sb.Append("</div>");//close propertyItem
            }
            if (!string.IsNullOrEmpty(container.UserAgentACL))
            {
                sb.Append("<div class=\"propertyItem\">");
                sb.Append("<div class=\"propertyItemheader\">User Agent ACL</div>");
                sb.Append("<div class=\"propertyItemContent\">");
                sb.AppendFormat("{0}", container.UserAgentACL);
                sb.Append("</div>");//close propertyItemContent
                sb.Append("</div>");//close propertyItem
            }*/

            sb.Append("<div class=\"propertyItem\">");
            sb.Append("<div class=\"propertyItemheader\">CDN Uri</div>");
            sb.Append("<div class=\"propertyItemContent\">");
            sb.AppendFormat("{0}", container.CdnUri);
            sb.Append("</div>");//close propertyItemContent
            sb.Append("</div>");//close propertyItem

            sb.Append("<div><div><span>");

            return sb.ToString();
        }

        private ListItem[] GetContainers()
        {
            EnsureConnection();
            var containers = _factory.GetAllContainersAsSimpleList();
            return containers.Select(container => new ListItem(container, container)).ToArray();
        }

        private Container GetContainerByName(string containerName)
        {
            EnsureConnection();
            return _factory.GetContainerInformation(containerName);
        }

        private void EnsureConnection()
        {
            if (_factory == null)
            {
                _factory = new Factory(Options.Username, Options.ApiKey);
            }
        }
    }
}
