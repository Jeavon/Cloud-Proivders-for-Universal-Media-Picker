using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitereactor.CloudProviders.Amazon.DataTypes.AmazonS3Uploader
{
    public class AS3U_Control : PlaceHolder
    {
        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>The options for the control.</value>
        public AS3U_Options Options { get; set; }

        protected FileUpload UploadField;
        protected Literal FileUrlField;

        public HttpPostedFile PostedFile
        {
            get { return this.UploadField.PostedFile; }
        }

        public string FileUrl { get; set; }

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
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.EnsureChildControls();

            // Video Id Literal Control
            if (this.FileUrlField == null)
            {
                this.FileUrlField = new Literal();
            }
            this.FileUrlField.ID = this.FileUrlField.ClientID;
            this.FileUrlField.Text = FileUrl;
            this.Controls.Add(this.FileUrlField);

            //Upload field as a HtmlInputFile
            if (this.UploadField == null)
            {
                this.UploadField = new FileUpload();
            }
            this.UploadField.ID = this.UploadField.ClientID;
            this.Controls.Add(this.UploadField);
        }

        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the server control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            // render the literal
            this.FileUrlField.RenderControl(writer);

            // render the upload field if no video id exists
            if (string.IsNullOrEmpty(FileUrl))
                this.UploadField.RenderControl(writer);
        }
    }
}
