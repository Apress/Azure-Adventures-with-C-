using Azure.Storage.Blobs;

namespace AzureAdventure4_Blobs
{
    public interface IBlobService
    {
        string AcquireLeaseOnBlob(string blobName);
        BlobContainerClient CreateBlobContainer(string containerName = "reports");
        void DeleteBlob(string blobName);
        byte[] DownloadBlob(string blobName);
        string DownloadTextFile(string blobName);
        List<string> GetBlobsInFolder(string folderName);
        string GetSasTokenForBlob(string blobName);
        void ReleaseLeaseOnBlob(string leaseId, string blobName);
        void UploadBlob(string blobName, Stream blobContent);
        void UploadBlob(string blobName, Stream blobContent, string leaseId);
    }
}