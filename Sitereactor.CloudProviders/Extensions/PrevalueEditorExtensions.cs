﻿using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitereactor.CloudProviders.Extensions
{
    /// <summary>
    /// Extension methods for the Prevalue Editor
    /// </summary>
    public static class PrevalueEditorExtensions
    {
        /// <summary>
        /// Adds the prevalue controls.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="controls">The controls.</param>
        public static void AddPrevalueControls(this ControlCollection collection, params Control[] controls)
        {
            foreach (Control control in controls)
            {
                collection.Add(control);
            }
        }

        /// <summary>
        /// Adds the prevalue row heading.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="heading">The heading.</param>
        public static void AddPrevalueHeading(this HtmlTextWriter writer, string heading)
        {
            writer.AddPrevalueHeading(heading, string.Empty);
        }

        public static void AddPrevalueHeading(this HtmlTextWriter writer, string heading, string note)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "row clearfix");
            writer.RenderBeginTag(HtmlTextWriterTag.Div); // start 'row'

            writer.RenderBeginTag(HtmlTextWriterTag.H3); // start 'h3'

            writer.Write(heading);

            writer.RenderEndTag(); // end 'h3'

            if (!string.IsNullOrEmpty(note))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "note");
                writer.RenderBeginTag(HtmlTextWriterTag.Div); // start 'note'

                writer.Write(note);

                writer.RenderEndTag(); // end 'note'
            }

            writer.RenderEndTag(); // end 'row'
        }

        /// <summary>
        /// Adds a new row to the Prevalue Editor.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter.</param>
        /// <param name="label">The label for the field.</param>
        /// <param name="controls">The controls for the field.</param>
        public static void AddPrevalueRow(this HtmlTextWriter writer, string label, params Control[] controls)
        {
            writer.AddPrevalueRow(label, string.Empty, controls);
        }

        /// <summary>
        /// Adds a new row to the Prevalue Editor, (with an optional description).
        /// </summary>
        /// <param name="writer">The HtmlTextWriter.</param>
        /// <param name="label">The label for the field.</param>
        /// <param name="description">The description for the field.</param>
        /// <param name="controls">The controls for the field.</param>
        public static void AddPrevalueRow(this HtmlTextWriter writer, string label, string description, params Control[] controls)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "row clearfix");
            writer.RenderBeginTag(HtmlTextWriterTag.Div); // start 'row'

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "label");
            writer.RenderBeginTag(HtmlTextWriterTag.Div); // start 'label'

            Label lbl = new Label() { Text = label };
            lbl.RenderControl(writer);

            writer.RenderEndTag(); // end 'label'

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "field");
            writer.RenderBeginTag(HtmlTextWriterTag.Div); // start 'field'

            foreach (Control control in controls)
            {
                control.RenderControl(writer);

            }

            writer.RenderEndTag(); // end 'field'

            if (!string.IsNullOrEmpty(description))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "description");
                writer.RenderBeginTag(HtmlTextWriterTag.Div); // start 'description'

                Label desc = new Label() { Text = description };
                desc.RenderControl(writer);

                writer.RenderEndTag(); // end 'description'
            }

            writer.RenderEndTag(); // end 'row'
        }

        /// <summary>
        /// Gets the CSS resource path for the Prevalue Editor.
        /// </summary>
        /// <param name="editor">The PreValue Editor.</param>
        /// <returns>Returns the resource path of the CSS for the Prevalue Editor.</returns>
        public static string GetCssResourcePath(this umbraco.interfaces.IDataPrevalue editor)
        {
            return "Sitereactor.CloudProviders.Resources.Styles.PrevalueEditor.css";
        }
    }
}
