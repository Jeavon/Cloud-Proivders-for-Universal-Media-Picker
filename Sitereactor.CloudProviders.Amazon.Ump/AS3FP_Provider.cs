using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using TheOutfield.UmbExt.UniversalMediaPicker;
using TheOutfield.UmbExt.UniversalMediaPicker.Extensions;
using TheOutfield.UmbExt.UniversalMediaPicker.Interfaces;
using TheOutfield.UmbExt.UniversalMediaPicker.Providers;

namespace Sitereactor.CloudProviders.Amazon.Ump
{
    public class AS3FP_Provider : AbstractProvider
    {
        private AS3FP_Config _config;
        private IConfigControl _configControl;
        private ICreateControl _createControl;
        private StorageFactory _factory;

        public AS3FP_Provider()
        {}

        public AS3FP_Provider(string config)
            : base(config)
        {
            _config = config.DeserializeJsonTo<AS3FP_Config>();
        }

        #region Overrides of AbstractProvider
        public override string Alias
        {
            get { return "Amazon S3 File Picker"; }
        }

        public override IConfigControl ConfigControl
        {
            get
            {
                if (_configControl == null)
                    _configControl = new AS3FP_ConfigControl(this, Config);

                return _configControl;
            }
        }

        public override ICreateControl CreateControl
        {
            get
            {
                if (_createControl == null)
                    _createControl = new AS3FP_CreateControl(this, Config);

                return _createControl;
            }
        }

        public override IList<MediaItem> GetMediaItems(string parentId)
        {
            var mediaItems = new List<MediaItem>();
            EnsureAmazonS3Connection();
            if (parentId == "-1")
            {
                var containers = _factory.GetAllBuckets();
                mediaItems.AddRange(containers.Select(container => container.ContainerToMediaItem()));
            }
            else
            {
                var items = _factory.GetAllObjectsByBucketName(parentId);
                mediaItems.AddRange(items.Select(item => GetMediaItemWithUrl(_config.CdnDomain, parentId, item.Key)));
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
            EnsureAmazonS3Connection();

            _factory.CreateObject(container, fileStream, contentType, fileName);

            return GetMediaItemById(fileName);
        }

        public ListItem[] GetContainers()
        {
            EnsureAmazonS3Connection();
            List<string> containers = _factory.GetAllBuckets();
            return containers.Select(container => new ListItem(container, container)).ToArray();
        }

        private void EnsureAmazonS3Connection()
        {
            if (_factory == null)
                _factory = new StorageFactory(_config.AccessKey, _config.SecretKey);
        }

        private static MediaItem GetMediaItemWithUrl(string customDomain, string bucketName, string itemName)
        {
            string url = string.IsNullOrEmpty(customDomain) ? string.Format("http://{0}.s3.amazonaws.com/{1}", bucketName, itemName) : string.Concat(customDomain, "/", itemName);
            return itemName.UrlToMediaItem(url);
        }
    }
}
