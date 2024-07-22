using Azure.Data.Tables;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;

namespace AzureAdventure8_Storage
{
    public interface IStorageService
    {
        BlobContainerClient GetBlobContainerClient();
        TableClient GetCloudTableClient();
        QueueClient GetQueueClient();
    }

    public class StorageService : IStorageService
    {
        public BlobContainerClient GetBlobContainerClient()
        {
            var blobServiceClient = new BlobServiceClient(new Uri("https://storageName.blob.core.windows.net"),
                new DefaultAzureCredential());

            return blobServiceClient.GetBlobContainerClient("container");
        }

        public QueueClient GetQueueClient()
        {
            return new QueueClient(new Uri("https://storageAccountName.queue.core.windows.net/queueName"), new DefaultAzureCredential());
        }

        public TableClient GetCloudTableClient()
        {
            return new TableClient(new Uri("https://accountName.table.core.windows.net/"), "tableName", new DefaultAzureCredential());
        }
    }
}
