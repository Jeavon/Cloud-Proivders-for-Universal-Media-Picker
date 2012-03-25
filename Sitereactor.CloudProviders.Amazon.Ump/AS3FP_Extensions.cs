using TheOutfield.UmbExt.UniversalMediaPicker;

namespace Sitereactor.CloudProviders.Amazon.Ump
{
    public static class AS3FP_Extensions
    {
        public static MediaItem ContainerToMediaItemFolder(this string container)
        {
            return new MediaItem
                       {
                           Id = container,
                           Title = container,
                           Icon = UmbracoIcons.FolderOpen,
                           HasChildren = true,
                           PreviewImageUrl = "",
                           Selectable = false
                       };
        }

        public static MediaItem ItemToMediaItemFolder(this string item, string bucketname)
        {
            string name = item.Remove(item.IndexOf('/'));

            return new MediaItem
                       {
                           Id = string.Concat(bucketname, "/", item),
                           Title = name,
                           Icon = UmbracoIcons.FolderOpen,
                           HasChildren = true,
                           PreviewImageUrl = "",
                           Selectable = false
                       };
        }

        public static MediaItem ItemToMediaItemFolder(this string item, string folderName, string bucketname)
        {
            return new MediaItem
            {
                Id = string.Concat(bucketname, "/", item),
                Title = folderName,
                Icon = UmbracoIcons.FolderOpen,
                HasChildren = true,
                PreviewImageUrl = "",
                Selectable = false
            };
        }

        public static MediaItem ItemToMediaItem(this string itemName)
        {
            return new MediaItem
                       {
                           Id = itemName,
                           Title = itemName,
                           Icon = UmbracoIcons.MediaFile,
                           HasChildren = false,
                           PreviewImageUrl = "",
                           Selectable = true
                       };
        }

        public static MediaItem UrlToMediaItem(this string itemName, string url)
        {
            string name = itemName.Substring(itemName.LastIndexOf('/') + 1);

            return new MediaItem
                       {
                           Id = url,
                           Title = name,
                           Icon = UmbracoIcons.MediaFile,
                           HasChildren = false,
                           PreviewImageUrl = url,
                           Selectable = true
                       };
        }
    }
}
