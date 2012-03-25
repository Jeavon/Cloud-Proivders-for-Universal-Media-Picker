using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Sitereactor.CloudProviders.Azure
{
    public class BlobFactory
    {
        private readonly CloudBlobClient _blobClient;

        public BlobFactory(string accountName, string key)
        {
            //Specify storage credentials.
            StorageCredentialsAccountAndKey credentials = new StorageCredentialsAccountAndKey(accountName, key);

            //Create a reference to your storage account, passing in your credentials.
            CloudStorageAccount storageAccount = new CloudStorageAccount(credentials, true);
            _blobClient = GetBlobClient(storageAccount);
        }

        /// <summary>
        /// Creates a Cloud Blob Client
        /// </summary>
        /// <param name="storageAccount"><see cref="CloudStorageAccount"/></param>
        /// <returns>The <see cref="CloudBlobClient"/></returns>
        private static CloudBlobClient GetBlobClient(CloudStorageAccount storageAccount)
        {
            //Create a new client object, which will provide access to Blob service resources.
            return storageAccount.CreateCloudBlobClient();
        }

        /// <summary>
        /// Gets a list of all containers
        /// </summary>
        /// <returns>A list of URIs as strings</returns>
        public List<string> GetAllContainers()
        {
            var blobContainers = _blobClient.ListContainers();
            return blobContainers.ToList().Select(c => c.Name.ToString()).ToList();
        }


        public List<CloudBlobContainer> GetAllBlobContainers()
        {
            var blobContainers = _blobClient.ListContainers();
            return blobContainers.ToList().Select(c => c).ToList();
        }

        /// <summary>
        /// Gets a list of blobs for a specified container
        /// </summary>
        /// <param name="containerAddress"></param>
        /// <returns>A list of URIs as strings</returns>
        public List<string> GetBlobsForContainer(string containerAddress)
        {
            CloudBlobContainer container = _blobClient.GetContainerReference(containerAddress);
            return container.ListBlobs().Select(b => b.Uri.LocalPath.ToString()).ToList();
        }

        /// <summary>
        /// Creates a new public blob container
        /// </summary>
        /// <param name="containerName">Name of Container</param>
        /// <returns>The Uri of the container</returns>
        public string CreatePublicContainer(string containerName)
        {
            //Create a new container.
            CloudBlobContainer container = _blobClient.GetContainerReference(containerName);
            container.CreateIfNotExist();

            //Specify that the container is publicly accessible.
            BlobContainerPermissions containerAccess = new BlobContainerPermissions();
            containerAccess.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(containerAccess);
            
            return container.Name;
        }

        /// <summary>
        /// Creates a new public blob
        /// </summary>
        /// <param name="containerName">Name of Container</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="fileStream">File as <see cref="Stream"/> to upload</param>
        /// <returns>The Uri of the file</returns>
        public string CreatePublicBlob(string containerName, string fileName, Stream fileStream)
        {
            string blobReference = string.Concat(containerName, "/", fileName);
            //Create a new blob and write some text to it.
            CloudBlob blob = _blobClient.GetBlobReference(blobReference);
            blob.UploadFromStream(fileStream);

            //Set the Cache-Control header on the blob.
            blob.SetCacheControl("public");

            return blob.Uri.ToString();
        }

        /// <summary>
        /// Creates a new public blob with refresh interval
        /// </summary>
        /// <param name="containerName">Name of Container</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="fileStream">File as <see cref="Stream"/> to upload</param>
        /// <param name="maxAge">Desired refresh interval</param>
        /// <returns>The Uri of the file</returns>
        public string CreatePublicBlob(string containerName, string fileName, Stream fileStream, string maxAge)
        {
            string blobReference = string.Concat(containerName, "/", fileName);
            //Create a new blob and write some text to it.
            CloudBlob blob = _blobClient.GetBlobReference(blobReference);
            blob.UploadFromStream(fileStream);

            //Set the Cache-Control header on the blob to specify your desired refresh interval.
            blob.SetCacheControl("public, max-age=" + maxAge);
            
            return blob.Uri.ToString();
        }
    }
}
