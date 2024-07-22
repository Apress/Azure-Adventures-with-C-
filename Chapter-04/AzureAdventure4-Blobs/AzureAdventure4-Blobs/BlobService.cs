using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;

namespace AzureAdventure4_Blobs
{
    public class BlobService : IBlobService
    {
        private readonly IConfiguration _configuration;

        public BlobService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public BlobContainerClient CreateBlobContainer(string containerName = "reports")
        {
            var blobServiceClient = new BlobServiceClient(_configuration.GetValue<string>("AzureWebJobsStorage"));
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            containerClient.CreateIfNotExists();

            return containerClient;
        }

        public void UploadBlob(string blobName, Stream blobContent)
        {
            var containerClient = CreateBlobContainer();
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            blobClient.Upload(blobContent, true);
        }

        public void UploadBlob(string blobName, Stream blobContent, string leaseId)
        {
            var containerClient = CreateBlobContainer();
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            blobClient.Upload(blobContent, new BlobUploadOptions
            {
                Conditions = new BlobRequestConditions
                {
                    LeaseId = leaseId
                }
            });
        }
        public byte[] DownloadBlob(string blobName)
        {
            var containerClient = CreateBlobContainer();
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            using MemoryStream ms = new MemoryStream();
            blobClient.DownloadTo(ms);
            return ms.ToArray();
        }

        public string DownloadTextFile(string blobName)
        {
            var containerClient = CreateBlobContainer();
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            var content = blobClient.DownloadContent();

            return content.Value.Content.ToString();
        }
        public void DeleteBlob(string blobName)
        {
            var containerClient = CreateBlobContainer();

            var blobClient = containerClient.GetBlobClient(blobName);
            blobClient.DeleteIfExists();
        }

        public List<string> GetBlobsInFolder(string folderName)
        {
            var containerClient = CreateBlobContainer();
            var blobs = containerClient.GetBlobs(prefix: folderName);

            return blobs.Select(blob => blob.Name).ToList();
        }

        public string GetSasTokenForBlob(string blobName)
        {
            var containerClient = CreateBlobContainer();

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobName = blobName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(2),
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            return blobClient.GenerateSasUri(sasBuilder).AbsoluteUri;
        }

        public string AcquireLeaseOnBlob(string blobName)
        {
            var containerClient = CreateBlobContainer();
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            BlobLeaseClient blobLeaseClient = blobClient.GetBlobLeaseClient();

            BlobLease lease = blobLeaseClient.Acquire(TimeSpan.FromSeconds(60));
            Console.WriteLine(lease.LeaseTime); 
            return lease.LeaseId;
        }

        public void ReleaseLeaseOnBlob(string leaseId, string blobName)
        {
            var containerClient = CreateBlobContainer();
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            BlobLeaseClient blobLeaseClient = blobClient.GetBlobLeaseClient(leaseId);
            blobLeaseClient.Release();
        }
    }
}
