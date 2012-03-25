using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TheOutfield.UmbExt.UniversalMediaPicker;
using TheOutfield.UmbExt.UniversalMediaPicker.Controls;
using TheOutfield.UmbExt.UniversalMediaPicker.Extensions;

namespace Sitereactor.CloudProviders.Azure.Ump
{
    public class AFP_CreateControl : AbstractCreateControl
    {
        private readonly AFP_Provider _provider;
        private AFP_Config _config;
        private readonly FileUpload _fileFileUpload = new FileUpload();
        private readonly TextBox _titleTextBox = new TextBox();
        private readonly TextBox _descriptionTextBox = new TextBox();
        private readonly TextBox _tagsTextBox = new TextBox();
        private readonly DropDownList _collectionsDropDownList = new DropDownList();

        public AFP_CreateControl(AFP_Provider provider, string options)
        {
            _provider = provider;
            _config = options.DeserializeJsonTo<AFP_Config>();
        }

        protected override void CreateChildControls()
        {
            _fileFileUpload.ID = "fileFileUpload";
            _titleTextBox.ID = "titleTextBox";
            _descriptionTextBox.ID = "descriptionTextBox";
            _tagsTextBox.ID = "tagsTextBox";
            _collectionsDropDownList.ID = "collectionsDropDownList";

            _fileFileUpload.CssClass = "guiInputText guiInputStandardSize";
            _titleTextBox.CssClass = "guiInputText guiInputStandardSize";
            _descriptionTextBox.CssClass = "guiInputText guiInputStandardSize";
            _tagsTextBox.CssClass = "guiInputText guiInputStandardSize";
            _collectionsDropDownList.CssClass = "guiInputText guiInputStandardSize";

            _descriptionTextBox.TextMode = TextBoxMode.MultiLine;
            _tagsTextBox.TextMode = TextBoxMode.MultiLine;

            Controls.Add(_fileFileUpload);
            Controls.Add(_titleTextBox);
            Controls.Add(_descriptionTextBox);
            Controls.Add(_tagsTextBox);
            Controls.Add(_collectionsDropDownList);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindCollections();
            }

            base.OnLoad(e);
        }

        protected void BindCollections()
        {
            _collectionsDropDownList.Items.Clear();
            _collectionsDropDownList.Items.Add(new ListItem("None", ""));
            _collectionsDropDownList.Items.AddRange(_provider.GetContainers());
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddFormRow("File:", _fileFileUpload);
            writer.AddFormRow("Container:", _collectionsDropDownList);
            writer.AddFormRow("Title:", _titleTextBox);
            writer.AddFormRow("Description:", _descriptionTextBox);
            writer.AddFormRow("Tags:", "Separated with commas", _tagsTextBox);
        }

        #region Overrides of AbstractCreateControl

        public override bool TrySave(out MediaItem savedMediaItem, out string message)
        {
            EnsureChildControls();

            if (_fileFileUpload.HasFile)
            {
                try
                {
                    savedMediaItem = _provider.CreateMediaItem(_fileFileUpload.FileContent, _fileFileUpload.FileName, _fileFileUpload.PostedFile.ContentType, _collectionsDropDownList.SelectedValue);

                    message = "File created successfully";

                    return true;
                }
                catch (Exception e)
                {
                    savedMediaItem = null;
                    message = "An error occured creating the media item: " + e.Message;

                    return false;
                }
            }
            else
            {
                savedMediaItem = null;
                message = "No file selected";

                return false;
            }
        }

        #endregion
    }
}
