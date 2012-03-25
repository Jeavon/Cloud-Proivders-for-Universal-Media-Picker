using System;
using System.Web.UI;
using Sitereactor.CloudProviders.Extensions;
using umbraco.cms.businesslogic.datatype;

namespace Sitereactor.CloudProviders.DataTypes.PrevalueEditors
{
    /// <summary>
    /// Overrides the default Prevalue Editor.
    /// </summary>
    public class DefaultPrevalueEditor : DefaultPreValueEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPrevalueEditor"/> class.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="displayTextBox">if set to <c>true</c> [display text box].</param>
        public DefaultPrevalueEditor(BaseDataType dataType, bool displayTextBox)
            : base(dataType, displayTextBox)
        {
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Adds the client dependencies.
            this.AddResourceToClientDependency(this.GetCssResourcePath(), ClientDependency.Core.ClientDependencyType.Css);
        }

        /// <summary>
        /// Renders the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "uCloud");
            writer.RenderBeginTag(HtmlTextWriterTag.Div); //// start 'uCloud'

            // render the child controls
            base.Render(writer);

            writer.RenderEndTag(); //// end 'uCloud'
        }
    }
}
