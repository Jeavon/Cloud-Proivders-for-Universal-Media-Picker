using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TheOutfield.UmbExt.UniversalMediaPicker;
using TheOutfield.UmbExt.UniversalMediaPicker.Controls;
using TheOutfield.UmbExt.UniversalMediaPicker.Extensions;

namespace Sitereactor.CloudProviders.Amazon.Ump
{
    public class AS3FP_CreateControl : AbstractCreateControl
    {
        private readonly AS3FP_Provider _provider;
        private AS3FP_Config _config;
        private readonly FileUpload _fileFileUpload = new FileUpload();
        private readonly TextBox _folderTextBox = new TextBox();
        private readonly DropDownList _collectionsDropDownList = new DropDownList();
        private readonly DropDownList _foldersDropDownList = new DropDownList();

        public AS3FP_CreateControl(AS3FP_Provider provider, string options)
        {
            _provider = provider;
            _config = options.DeserializeJsonTo<AS3FP_Config>();
        }

        protected override void CreateChildControls()
        {
            _fileFileUpload.ID = "fileFileUpload";
            _folderTextBox.ID = "folderTextBox";
            _collectionsDropDownList.ID = "collectionsDropDownList";
            _foldersDropDownList.ID = "foldersDropDownList";

            _fileFileUpload.CssClass = "guiInputText guiInputStandardSize";
            _folderTextBox.CssClass = "guiInputText guiInputStandardSize";
            _collectionsDropDownList.CssClass = "guiInputText guiInputStandardSize";
            _foldersDropDownList.CssClass = "guiInputText guiInputStandardSize";

            Controls.Add(_fileFileUpload);
            Controls.Add(_folderTextBox);
            Controls.Add(_collectionsDropDownList);
            Controls.Add(_foldersDropDownList);
        }

        protected override void OnLoad(EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                BindCollections();
            }

            base.OnLoad(e);
        }

        protected void BindCollections()
        {
            _collectionsDropDownList.Items.Clear();
            _collectionsDropDownList.Items.Add(new ListItem("", ""));
            _collectionsDropDownList.Items.AddRange(_provider.GetContainers());
            _collectionsDropDownList.DataBind();

            _foldersDropDownList.Items.Clear();
            _foldersDropDownList.Items.Add(new ListItem("Root", ""));
            _foldersDropDownList.Items.AddRange(_provider.GetFolders());
            _foldersDropDownList.DataBind();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddFormRow("File*:", _fileFileUpload);
            writer.AddFormRow("Bucket*:", _collectionsDropDownList);
            writer.AddFormRow("Parent Folder:", _foldersDropDownList);
            writer.AddFormRow("New Folder:", _folderTextBox);
        }

        #region Overrides of AbstractCreateControl

        public override bool TrySave(out MediaItem savedMediaItem, out string message)
        {
            EnsureChildControls();

            if (string.IsNullOrEmpty(_collectionsDropDownList.SelectedValue))
            {
                message = "No Bucket was selected. Please select a bucket and try again.";
                savedMediaItem = null;
                return false;
            }

            if (_fileFileUpload.HasFile)
            {
                try
                {
                    string folder = _folderTextBox.Text;
                    string fileName = _fileFileUpload.FileName;
                    string parentFolder = _foldersDropDownList.SelectedValue;

                    if (!string.IsNullOrEmpty(folder))
                    {
                        fileName = folder.LastIndexOf('/') == folder.Length - 1
                                       ? string.Concat(parentFolder, folder, _fileFileUpload.FileName)
                                       : string.Concat(parentFolder, folder, "/", _fileFileUpload.FileName);

                        _provider.CreateFolderBeforeUpload(fileName.Replace(_fileFileUpload.FileName, ""), _collectionsDropDownList.SelectedValue);
                    }
                    else if(!string.IsNullOrEmpty(parentFolder))
                    {
                        fileName = string.Concat(parentFolder, _fileFileUpload.FileName);
                    }

                    savedMediaItem = _provider.CreateMediaItem(_fileFileUpload.FileContent, 
                        fileName, 
                        _fileFileUpload.PostedFile.ContentType, 
                        _collectionsDropDownList.SelectedValue);

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
