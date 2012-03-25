using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Rackspace.Cloudfiles;
using TheOutfield.UmbExt.UniversalMediaPicker;
using TheOutfield.UmbExt.UniversalMediaPicker.Extensions;
using TheOutfield.UmbExt.UniversalMediaPicker.Interfaces;
using TheOutfield.UmbExt.UniversalMediaPicker.Providers;

namespace Sitereactor.CloudProviders.CloudFiles.Ump
{
    public class CFFP_Provider : AbstractProvider
    {
        private CFFP_Config _config;
        private IConfigControl _configControl;
        private ICreateControl _createControl;
        private Factory _factory;

        public CFFP_Provider()
        {}

        public CFFP_Provider(string config) : base(config)
        {
            _config = config.DeserializeJsonTo<CFFP_Config>();
        }

        #region Overrides of AbstractProvider
        public override string Alias
        {
            get { return "Rackspace Cloud Files"; }
        }

        public override IConfigControl ConfigControl
        {
            get
            {
                if (_configControl == null)
                    _configControl = new CFFP_ConfigControl(this, Config);

                return _configControl;
            }
        }

        public override ICreateControl CreateControl
        {
            get
            {
                if (_createControl == null)
                    _createControl = new CFFP_CreateControl(this, Config);

                return _createControl;
            }
        }

        public override IList<MediaItem> GetMediaItems(string parentId)
        {
            var mediaItems = new List<MediaItem>();
            EnsureCloudFilesConnection();
            if (parentId == "-1")
            {
                var containers = _factory.GetAllContainersAsSimpleList();
                mediaItems.AddRange(containers.Select(container => container.ContainerToMediaItem()));
            }
            else
            {
                var items = _factory.GetAllItemsByContainer(parentId);
                string uriForContainer = _factory.GetCdnUriForContainer(parentId);
                mediaItems.AddRange(items.Select(item => GetMediaItemWithUrlAsId(uriForContainer, item)));
            }

            return mediaItems;
        }

        public override MediaItem GetMediaItemById(string id)
        {
            // Connect to API, an retrive items
            int lastSlash = id.LastIndexOf("/") + 1;
            string itemName = id.Substring(lastSlash);

            return id.ToMediaItem(itemName);
        }
        #endregion

        public MediaItem CreateMediaItem(Stream fileStream, string fileName, string title, string description, string tags, string container)
        {
            EnsureCloudFilesConnection();

            var dictionary = new Dictionary<string, string>();
            if(!string.IsNullOrEmpty(title))
                dictionary.Add("Title", title);
            if (!string.IsNullOrEmpty(description))
                dictionary.Add("Desciption", description);
            if (!string.IsNullOrEmpty(tags))
                dictionary.Add("Tags", tags);

            if (dictionary.Any())
            {
                _factory.PutItemInContainer(dictionary, container, fileStream, fileName);
            }
            else
            {
                _factory.PutItemInContainer(container, fileStream, fileName);
            }

            string fileId = string.Concat(container, "|", fileName);
            return GetMediaItemById(fileId);
        }

        public ListItem[] GetContainers()
        {
            EnsureCloudFilesConnection();
            var containers = _factory.GetAllContainersAsSimpleList();
            return containers.Select(container => new ListItem(container, container)).ToArray();
        }

        private void EnsureCloudFilesConnection()
        {
            if (_factory == null)
                _factory = new Factory(_config.Username, _config.ApiKey);
        }

        private static MediaItem GetMediaItemWithUrlAsId(string uriForContainer, StorageObject itemName)
        {
            string cdnUriForItem = string.Concat(uriForContainer, itemName.Name);
            return cdnUriForItem.UriToMediaItem(itemName);
        }
    }
}
