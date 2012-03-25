using Microsoft.WindowsAzure.StorageClient;

namespace Sitereactor.CloudProviders.Azure
{
    public static class CloudExtensions
    {
            //A convenience method to set the Cache-Control header.
        public static void SetCacheControl(this CloudBlob blob, string value)
        {
            blob.Properties.CacheControl = value;
            blob.SetProperties();
        }
    }
}
