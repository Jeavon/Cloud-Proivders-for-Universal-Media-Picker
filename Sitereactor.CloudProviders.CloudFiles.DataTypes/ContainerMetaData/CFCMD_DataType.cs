using System;
using Sitereactor.CloudProviders.Extensions;
using umbraco.cms.businesslogic.datatype;
using umbraco.interfaces;

namespace Sitereactor.CloudProviders.CloudFiles.DataTypes.ContainerMetaData
{
    public class CFCMD_DataType : AbstractDataEditor
    {
        /// <summary>
        /// The render control.
        /// </summary>
        private CFCMD_Control m_Control = new CFCMD_Control();

        /// <summary>
        /// The Data object for the data-type.
        /// </summary>
        private IData m_Data;

        /// <summary>
        /// The PreValue Editor for the data-type.
        /// </summary>
        private CFCMD_PrevalueEditor m_PreValueEditor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CFCMD_DataType"/> class.
        /// </summary>
        public CFCMD_DataType()
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
                    this.m_Data = new CFCMD_Data(this);
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
                return "uCloud CDN Providers: Rackspace Cloud Files - Container";
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
                return new Guid("6180b323-938c-481e-9681-65b2b50bcd8f");
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
                    this.m_PreValueEditor = new CFCMD_PrevalueEditor(this);
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
            var options = ((CFCMD_PrevalueEditor)this.PrevalueEditor).GetPreValueOptions<CFCMD_Options>();

            if (this.Data.Value != null)
            {
                this.m_Control.ControlID = this.DataEditor.Editor.ID.AddDefaultAlias();
                this.m_Control.ContainerName = string.IsNullOrEmpty(options.DefaultContainer) ? this.Data.Value.ToString() : options.DefaultContainer;
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
            // save the value of the control depending on whether a new file is uploaded);
            this.Data.Value = this.m_Control.ContainerName;
        }
    }
}
