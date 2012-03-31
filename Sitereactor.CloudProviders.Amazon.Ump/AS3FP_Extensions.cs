using TheOutfield.UmbExt.UniversalMediaPicker;

namespace Sitereactor.CloudProviders.Amazon.Ump
{
    public static class AS3FP_Extensions
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
            return new MediaItem
            {
                Id = url,
                Title = itemName,
                Icon = UmbracoIcons.MediaFile,
                HasChildren = false,
                PreviewImageUrl = url,
                Selectable = true
            };
        }
    }
}
