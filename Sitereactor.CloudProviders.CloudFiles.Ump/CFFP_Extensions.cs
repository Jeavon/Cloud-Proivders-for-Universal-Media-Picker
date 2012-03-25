using Rackspace.Cloudfiles;
using TheOutfield.UmbExt.UniversalMediaPicker;

namespace Sitereactor.CloudProviders.CloudFiles.Ump
{
    public static class CFFP_Extensions
    {
        public static MediaItem ContainerToMediaItem(this string container)
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

        public static MediaItem UriToMediaItem(this string uri, StorageObject item)
        {
            return new MediaItem
            {
                Id = uri,
                Title = item.Name,
                Icon = UmbracoIcons.MediaFile,
                HasChildren = false,
                PreviewImageUrl = uri,
                Selectable = true,
                MetaData = item.Metadata
            };
        }

        public static MediaItem ToMediaItem(this string uri, string itemName)
        {
            return new MediaItem
            {
                Id = uri,
                Title = itemName,
                Icon = UmbracoIcons.MediaFile,
                HasChildren = false,
                PreviewImageUrl = uri,
                Selectable = true
            };
        }
    }
}
