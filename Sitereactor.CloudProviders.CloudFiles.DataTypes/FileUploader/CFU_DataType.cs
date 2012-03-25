using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Sitereactor.CloudProviders.Extensions;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.datatype;
using umbraco.interfaces;

namespace Sitereactor.CloudProviders.CloudFiles.DataTypes.FileUploader
{
    public class CFU_DataType : AbstractDataEditor
    {
        /// <summary>
        /// The render control.
        /// </summary>
        private CFU_Control m_Control = new CFU_Control();

        /// <summary>
        /// The Data object for the data-type.
        /// </summary>
        private IData m_Data;

        /// <summary>
        /// The PreValue Editor for the data-type.
        /// </summary>
        private CFU_PrevalueEditor m_PreValueEditor;

        /// <summary>
        /// The Container Name used for events
        /// </summary>
        private string ContainerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="CFU_DataType"/> class.
        /// </summary>
        public CFU_DataType()
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
                    this.m_Data = new CFU_Data(this);
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
                return "uCloud CDN Providers: Rackspace Cloud Files - Uploader";
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
                return new Guid("6ffe42fd-d4c1-47d2-b437-6e65f7171923");
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
                    this.m_PreValueEditor = new CFU_PrevalueEditor(this);
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
            var options = ((CFU_PrevalueEditor)this.PrevalueEditor).GetPreValueOptions<CFU_Options>();

            if (this.Data.Value != null)
            {
                this.m_Control.ControlID = this.DataEditor.Editor.ID.AddDefaultAlias();
                this.m_Control.FileUrl = this.Data.Value.ToString();

                var dictionary = this.Data.Value.ToString().DeserializeJsonTo<Dictionary<string, string>>();
                if (dictionary != null)
                {
                    this.m_Control.FileUrl = dictionary.ContainsKey("FileUrl") ? dictionary["FileUrl"] : string.Empty;
                    this.m_Control.ContainerName = dictionary.ContainsKey("ContainerName") ? dictionary["ContainerName"] : string.Empty;
                }
            }

            // set the render control options
            this.m_Control.Options = options;
            this.m_Control.Data = Data as DefaultData;
        }

        /// <summary>
        /// Saves the editor control value.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void DataEditorControl_OnSave(EventArgs e)
        {
            SaveEventArgs eventArgs = new SaveEventArgs();
            FireBeforeSave(eventArgs);

            if(eventArgs.Cancel) return;

            string fileUrl = string.Empty;
            string containerName = string.Empty;
            var postedFile = this.m_Control.PostedFile;

            if(postedFile != null)
            {
                Factory factory = new Factory(m_Control.Options.Username, this.m_Control.Options.ApiKey);
                
                if (!string.IsNullOrEmpty(ContainerName))
                {
                    //Get containerName from private ContainerName, which is set by a beforesave event.
                    containerName = ContainerName;
                }
                else if(!string.IsNullOrEmpty(m_Control.Options.DefaultContainer))
                {
                    //Get default container name from prevalues.
                    containerName = m_Control.Options.DefaultContainer;
                }
                else if (!string.IsNullOrEmpty(m_Control.Options.DefaultContainerAlias))
                {
                    //Get containerName by posted container name via prevalue default alias.
                    string defaultAlias = m_Control.Options.DefaultContainerAlias.AddDefaultAlias();
                    NameValueCollection form = HttpContext.Current.Request.Form;
                    foreach (string s in form.AllKeys.Where(s => s.Contains(defaultAlias)))
                    {
                        containerName = form.Get(s);
                    }
                }
                //Check if the Container Name was actually set along the way before trying to upload
                if (!string.IsNullOrEmpty(containerName))
                {
                    //upload file to container
                    factory.PutItemInContainer(containerName, postedFile.InputStream, postedFile.FileName);
                    //get the url of the file in CDN
                    fileUrl = factory.GetCdnUriForItem(containerName, postedFile.FileName);
                }
            }

            var dictionary = new Dictionary<string, string>
                                 {
                                     {"FileUrl", string.IsNullOrEmpty(fileUrl) ? m_Control.FileUrl : fileUrl},
                                     {"ContainerName", containerName}
                                 };

            // save the value of the control depending on whether a new file is uploaded););
            this.Data.Value = dictionary.SerializeToJson();

            FireAfterSave(eventArgs);
        }

        #region Events
        /// <summary>
        /// The save event handler
        /// </summary>
        public delegate void SaveEventHandler(IData sender, ref string containerName, SaveEventArgs e);

        /// <summary>
        /// Occurs when [before save].
        /// </summary>
        public new static event SaveEventHandler BeforeSave;
        /// <summary>
        /// Raises the <see cref="E:BeforeSave"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected new virtual void FireBeforeSave(SaveEventArgs e)
        {
            if (BeforeSave != null)
                BeforeSave(this.m_Data, ref this.ContainerName, e);
        }

        /// <summary>
        /// Occurs when [after save].
        /// </summary>
        public new static event SaveEventHandler AfterSave;
        /// <summary>
        /// Raises the <see cref="E:AfterSave"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected new virtual void FireAfterSave(SaveEventArgs e)
        {
            if (AfterSave != null)
                AfterSave(this.m_Data, ref this.ContainerName, e);
        }
        #endregion
    }
}
