using System;
using umbraco.cms.businesslogic.datatype;
using umbraco.interfaces;

namespace Sitereactor.CloudProviders.Amazon.DataTypes.AmazonS3Uploader
{
    public class AS3U_DataType : AbstractDataEditor
    {
        /// <summary>
        /// The render control.
        /// </summary>
        private AS3U_Control m_Control = new AS3U_Control();

        /// <summary>
        /// The Data object for the data-type.
        /// </summary>
        private IData m_Data;

        /// <summary>
        /// The PreValue Editor for the data-type.
        /// </summary>
        private AS3U_PrevalueEditor m_PreValueEditor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AS3U_DataType"/> class.
        /// </summary>
        public AS3U_DataType()
            : base()
        {
            // set the render control as the placeholder
            this.RenderControl = this.m_Control;

            // assign the initialise event for the placeholder
            this.m_Control.Init += new EventHandler(this.m_Control_Init);

            // assign the save event for the data-type/editor
            this.DataEditorControl.OnSave += new AbstractDataEditorControl.SaveEventHandler(this.DataEditorControl_OnSave);
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>The data.</value>
        public override IData Data
        {
            get
            {
                if (this.m_Data == null)
                {
                    this.m_Data = new AS3U_Data(this);
                }

                return this.m_Data;
            }
        }

        /// <summary>
        /// Gets the name of the data type.
        /// </summary>
        /// <value>The name of the data type.</value>
        public override string DataTypeName
        {
            get
            {
                return "uCloud Providers: Amazon S3 File Uploader";
            }
        }

        /// <summary>
        /// Gets the id of the data-type.
        /// </summary>
        /// <value>The id of the data-type.</value>
        public override Guid Id
        {
            get
            {
                return new Guid("44565d5e-1b0c-4cdc-aed4-c478d9931fc4");
            }
        }

        /// <summary>
        /// Gets the prevalue editor.
        /// </summary>
        /// <value>The prevalue editor.</value>
        public override IDataPrevalue PrevalueEditor
        {
            get
            {
                if (this.m_PreValueEditor == null)
                {
                    this.m_PreValueEditor = new AS3U_PrevalueEditor(this);
                }

                return this.m_PreValueEditor;
            }
        }

        /// <summary>
        /// Handles the Init event of the m_Placeholder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void m_Control_Init(object sender, EventArgs e)
        {
            // get the render control options from the Prevalue Editor.
            var options = ((AS3U_PrevalueEditor)this.PrevalueEditor).GetPreValueOptions<AS3U_Options>();

            if (this.Data.Value != null)
            {
                this.m_Control.FileUrl = this.Data.Value.ToString();
            }

            // set the render control options
            this.m_Control.Options = options;
        }

        /// <summary>
        /// Saves the editor control value.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void DataEditorControl_OnSave(EventArgs e)
        {
            string fileUrl = string.Empty;
            string bucketName = string.Empty;
            var postedFile = this.m_Control.PostedFile;

            if (postedFile != null)
            {
                StorageFactory factory = new StorageFactory(m_Control.Options.AccessKey, m_Control.Options.SecretKey);
                fileUrl = factory.CreateObject(bucketName, postedFile.InputStream, postedFile.ContentType, postedFile.FileName);
            }

            // save the value of the control depending on whether a new file is uploaded
            this.Data.Value = string.IsNullOrEmpty(fileUrl) ? m_Control.FileUrl : fileUrl;
        }
    }
}
