﻿using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rackspace.Cloudfiles;
using Sitereactor.CloudProviders.Extensions;

namespace Sitereactor.CloudProviders.CloudFiles.DataTypes.CreateContainer
{
    public class CFCC_Control : PlaceHolder
    {
        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>The options for the control.</value>
        public CFCC_Options Options { get; set; }

        protected TextBox ContainerInputBox = new TextBox();
        protected CheckBox ContainerPublic = new CheckBox();
        protected Literal MetaDataLiteral = new Literal();

        private Factory _factory;
        private string _containerName;
        private bool _isPublic;

        public string ContainerName
        {
            get { return ContainerInputBox.Text; }
            set { _containerName = value; }
        }
        public bool IsPublic
        {
            get { return ContainerPublic.Checked; } 
            set { _isPublic = value; }
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

            // Textbox Control
            this.ContainerInputBox.ID = ControlID;
            this.ContainerInputBox.Text = _containerName;
            //Checkbox Control
            this.ContainerPublic.ID = this.ContainerPublic.ClientID;
            this.ContainerPublic.Text = "Public";
            this.ContainerPublic.Checked = _isPublic;
            //Literal control with meta data for Container
            this.MetaDataLiteral.ID = this.MetaDataLiteral.ClientID;
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.EnsureChildControls();
            
            this.Controls.Add(this.ContainerInputBox);
            this.Controls.Add(this.ContainerPublic);
            this.Controls.Add(this.MetaDataLiteral);
        }

        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the server control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            this.ContainerInputBox.RenderControl(writer);
            this.ContainerPublic.RenderControl(writer);

            if (!string.IsNullOrEmpty(ContainerInputBox.Text) && Options.ShowMetaData)
                this.MetaDataLiteral.Text = PopulateWithMetaData(ContainerInputBox.Text);

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
