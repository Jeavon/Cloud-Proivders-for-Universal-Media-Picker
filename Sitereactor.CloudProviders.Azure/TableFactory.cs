using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Sitereactor.CloudProviders.Azure
{
    public class TableFactory
    {
        private CloudTableClient _tableClient;

        public TableFactory(string accountName, string key)
        {
            //Specify storage credentials.
            StorageCredentialsAccountAndKey credentials = new StorageCredentialsAccountAndKey(accountName, key);

            //Create a reference to your storage account, passing in your credentials.
            CloudStorageAccount storageAccount = new CloudStorageAccount(credentials, true);
            _tableClient = GetTableClient(storageAccount);
        }

        private static CloudTableClient GetTableClient(CloudStorageAccount storageAccount)
        {
            //Create a new client object, which will provide access to Table service resources.
            return storageAccount.CreateCloudTableClient();
        }
    }
}
