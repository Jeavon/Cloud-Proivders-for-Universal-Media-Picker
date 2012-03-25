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
                List<string> containers = _factory.GetAllBuckets();
                //limit list of buckets to selected allowed ones if any exists in config
                mediaItems.AddRange(_config.AllowedBuckets.Any()
                                        ? containers.Where(x => _config.AllowedBuckets.Contains(x)).Select(
                                            container => container.ContainerToMediaItemFolder())
                                        : containers.Select(container => container.ContainerToMediaItemFolder()));
            }
            else
            {
                if (parentId.Contains("/"))
                {
                    var list = parentId.Split('/');
                    string bucketName = list[0];
                    string currentPath = parentId.Substring(parentId.IndexOf('/') + 1);

                    var items = _factory.GetAllObjectsByBucketName(bucketName);
                    foreach (var item in items)
                    {
                        //This should be a file
                        if (item.Key.Contains(currentPath) && item.Key.LastIndexOf('/') != item.Key.Length - 1)
                        {
                            //Match begining of file path
                            if (currentPath.LastIndexOf('/') == item.Key.LastIndexOf('/'))
                            {
                                mediaItems.Add(GetMediaItemWithUrl(_config.CdnDomain, parentId, item.Key));
                            }
                        }
                        //This should be a folder
                        if (item.Key.StartsWith(currentPath) && item.Key.LastIndexOf('/') == item.Key.Length - 1 && !currentPath.Equals(item.Key))
                        {
                            var folders = item.Key.Split('/');
                            var folderName = folders[folders.Length - 2];
                            mediaItems.Add(item.Key.ItemToMediaItemFolder(folderName, bucketName));
                        }
                    }
                }
                else
                {
                    var items = _factory.GetAllObjectsByBucketName(parentId);

                    mediaItems.AddRange(from item in items
                                        where item.Key.Count(f => f == '/') == 1 && item.Key.LastIndexOf('/') == item.Key.Length - 1
                                        select item.Key.ItemToMediaItemFolder(parentId));

                    mediaItems.AddRange(from item in items
                                        where item.Value != 0 && item.Key.Count(f => f == '/') == 0
                                        select GetMediaItemWithUrl(_config.CdnDomain, parentId, item.Key));
                }
            }

            return mediaItems;
        }

        public override MediaItem GetMediaItemById(string id)
        {
            int lastSlash = id.LastIndexOf("/") + 1;
            string itemName = id.Substring(lastSlash);

            return itemName.ItemToMediaItem();
        }
        #endregion

        public MediaItem CreateMediaItem(Stream fileStream, string fileName, string contentType, string bucketName)
        {
            EnsureAmazonS3Connection();

            _factory.CreateObject(bucketName, fileStream, contentType, fileName);

            return GetMediaItemWithUrl(_config.CdnDomain, bucketName, fileName);
        }

        public void CreateFolderBeforeUpload(string folderName, string bucketName)
        {
            EnsureAmazonS3Connection();

            var items = _factory.GetAllObjectsByBucketName(bucketName);
            if(!items.ContainsKey(folderName))
            {
                _factory.CreateFolderAsObject(bucketName, folderName);
            }
        }

        public ListItem[] GetContainers()
        {
            EnsureAmazonS3Connection();
            List<string> containers = _factory.GetAllBuckets();

            if (_config.AllowedBuckets.Any())
            {
                return containers.Where(x => _config.AllowedBuckets.Contains(x)).Select(
                    container => new ListItem(container, container)).ToArray();
            }

            return containers.Select(container => new ListItem(container, container)).ToArray();
        }

        public ListItem[] GetFolders()
        {
            EnsureAmazonS3Connection();
            
            List<ListItem> list = new List<ListItem>();
            var buckets = _factory.GetAllBuckets();
            foreach (var bucket in buckets)
            {
                var items = _factory.GetAllObjectsByBucketName(bucket);

                list.AddRange(from item in items
                              where item.Key.LastIndexOf('/') == item.Key.Length - 1
                              select new ListItem(string.Concat(item, " - Bucket: ", bucket), item.Key));
            }

            return list.ToArray();
        }

        private void EnsureAmazonS3Connection()
        {
            if (_factory == null)
                _factory = new StorageFactory(_config.AccessKey, _config.SecretKey);
        }

        private static MediaItem GetMediaItemWithUrl(string customDomain, string bucketName, string itemName)
        {
            string url = string.IsNullOrEmpty(customDomain) ? 
                string.Format("http://{0}.s3.amazonaws.com/{1}", bucketName, itemName) : 
                string.Concat(customDomain, "/", itemName);
            return itemName.UrlToMediaItem(url);
        }
    }
}
