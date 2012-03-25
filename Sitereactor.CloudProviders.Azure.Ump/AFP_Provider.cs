using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using TheOutfield.UmbExt.UniversalMediaPicker;
using TheOutfield.UmbExt.UniversalMediaPicker.Extensions;
using TheOutfield.UmbExt.UniversalMediaPicker.Interfaces;
using TheOutfield.UmbExt.UniversalMediaPicker.Providers;

namespace Sitereactor.CloudProviders.Azure.Ump
{
    public class AFP_Provider : AbstractProvider
    {
        private AFP_Config _config;
        private IConfigControl _configControl;
        private ICreateControl _createControl;
        private BlobFactory _factory;

        public AFP_Provider()
        {}

        public AFP_Provider(string config)
            : base(config)
        {
            _config = config.DeserializeJsonTo<AFP_Config>();
        }

        #region Overrides of AbstractProvider
        public override string Alias
        {
            get { return "Azure CDN File Picker"; }
        }

        public override IConfigControl ConfigControl
        {
            get
            {
                if (_configControl == null)
                    _configControl = new AFP_ConfigControl(this, Config);

                return _configControl;
            }
        }

        public override ICreateControl CreateControl
        {
            get
            {
                if (_createControl == null)
                    _createControl = new AFP_CreateControl(this, Config);

                return _createControl;
            }
        }

        public override IList<MediaItem> GetMediaItems(string parentId)
        {
            var mediaItems = new List<MediaItem>();
            EnsureAzureConnection();
            if (parentId == "-1")
            {
                List<string> containers = _factory.GetAllContainers();
                mediaItems.AddRange(containers.Select(container => container.ContainerToMediaItem()));
            }
            else
            {
                List<string> items = _factory.GetBlobsForContainer(parentId);
                mediaItems.AddRange(items.Select(item => GetMediaItemWithUrl(_config.CdnDomain, item)));
            }

            return mediaItems;
        }

        public override MediaItem GetMediaItemById(string id)
        {
            // Connect to API, an retrive items
            int lastSlash = id.LastIndexOf("/") + 1;
            string itemName = id.Substring(lastSlash);

            return itemName.ItemToMediaItem();
        }
        #endregion

        public MediaItem CreateMediaItem(Stream fileStream, string fileName, string contentType, string container)
        {
            EnsureAzureConnection();

            _factory.CreatePublicBlob(container, fileName, fileStream);

            return GetMediaItemById(fileName);
        }

        public ListItem[] GetContainers()
        {
            EnsureAzureConnection();
            List<string> containers = _factory.GetAllContainers();
            return containers.Select(container => new ListItem(container, container)).ToArray();
        }

        private void EnsureAzureConnection()
        {
            if (_factory == null)
                _factory = new BlobFactory(_config.AccountName, _config.Key);
        }

        private static MediaItem GetMediaItemWithUrl(string domain, string itemName)
        {
            string url = string.Concat(domain.TrimEnd("/".ToCharArray()), itemName);
            return itemName.UrlToMediaItem(url);
        }
    }
}
