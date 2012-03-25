using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.cms.businesslogic.datatype;
using umbraco.cms.businesslogic.property;

namespace Sitereactor.CloudProviders.CloudFiles.DataTypes.FileUploader
{
    public class CFU_Control : PlaceHolder
    {
        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>The options for the control.</value>
        public CFU_Options Options { get; set; }
        public DefaultData Data { get; set; }

        protected FileUpload UploadField = new FileUpload();
        protected Literal FileUrlField = new Literal();
        protected ImageButton ImageBtnDelete = new ImageButton();

        public HttpPostedFile PostedFile
        {
            get { return this.UploadField.PostedFile; }
        }

        public string FileUrl { get; set; }
        public string ContainerName { get; set; }
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

            //Upload field as a HtmlInputFile
            this.UploadField.ID = this.ControlID;
            // File Literal Control
            this.FileUrlField.ID = this.FileUrlField.ClientID;
            //Delete button
            this.ImageBtnDelete.ID = this.ImageBtnDelete.ClientID;
            // Adds the client dependencies.
            this.ImageBtnDelete.ImageUrl = "/umbraco/images/delete.small.png";
            this.ImageBtnDelete.Click += new ImageClickEventHandler(ImageBtnDelete_Click);
        }

        void ImageBtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(FileUrl) && !string.IsNullOrEmpty(ContainerName))
            {
                //Trim Url down to filename
                int lastSlash = FileUrl.LastIndexOf("/") + 1;
                string fileName = FileUrl.Substring(lastSlash);
                
                //Delete file from container here
                Factory factory = new Factory(Options.Username, Options.ApiKey);
                factory.DeleteItemFromContainer(ContainerName, fileName);

                //TODO Need to trigger Save() for datatype to remove saved data from property
                //NOTE Maybe set the ID from datatype to this control, load Content object and set property-alias-value to string.empty. But where does the alias come from?
                if(Data != null)
                {
                    //Clear the current datatypes property
                    Property property = new Property(Data.PropertyId) {Value = string.Empty};
                    //Clear file from controls properties
                    FileUrl = string.Empty;
                    FileUrlField.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.EnsureChildControls();
            
            this.Controls.Add(this.UploadField);
            this.Controls.Add(this.FileUrlField);
            this.Controls.Add(this.ImageBtnDelete);
        }

        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the server control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            //Set the literal text property to the url of the file
            this.FileUrlField.Text = FileUrl;

            // render the upload field if file url exists
            if (string.IsNullOrEmpty(FileUrlField.Text))
            {
                this.UploadField.RenderControl(writer);
            }
            else
            {
                // render the literal with file url and delete button
                this.FileUrlField.RenderControl(writer);
                writer.Write("</span><span style=\"padding-left:12px;\">");
                this.ImageBtnDelete.RenderControl(writer);
                //Show Meta Data if set in prevalue editor
                if(Options.ShowMetaData)
                    writer.Write(PopulateWithMetaData());
            }
        }

        private string PopulateWithMetaData()
        {
            if (string.IsNullOrEmpty(ContainerName) || string.IsNullOrEmpty(FileUrlField.Text)) return string.Empty;

            //Trim Url down to filename
            int lastSlash = FileUrl.LastIndexOf("/") + 1;
            string fileName = FileUrl.Substring(lastSlash);

            Factory factory = new Factory(Options.Username, Options.ApiKey);
            var cloudFile = factory.GetStorageItemInformation(ContainerName, fileName);

            StringBuilder sb = new StringBuilder();
            sb.Append("</span>");
            sb.Append("</div>");//close propertyItemContent
            sb.Append("</div>");//close propertyItem

            sb.Append("<div class=\"propertyItem\">");
            sb.Append("<div class=\"propertyItemheader\">Content Type</div>");
            sb.Append("<div class=\"propertyItemContent\">");
            sb.Append(cloudFile.ContentType);
            sb.Append("</div>");//close propertyItemContent
            sb.Append("</div>");//close propertyItem

            sb.Append("<div class=\"propertyItem\">");
            sb.Append("<div class=\"propertyItemheader\">Content Length</div>");
            sb.Append("<div class=\"propertyItemContent\">");
            sb.Append(cloudFile.ContentLength);
            sb.Append("</div>");//close propertyItemContent
            sb.Append("</div>");//close propertyItem

            sb.Append("<div><div><span>");

            return sb.ToString();
        }
    }
}
