using System.Net;
using System.Text;
using Azure;
using HttpMultipartParser;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureAdventure4_Blobs
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly IBlobService _blobService;

        public Function1(ILoggerFactory loggerFactory, IBlobService blobService)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _blobService = blobService;
        }

        [Function("Function1")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            _blobService.CreateBlobContainer();

            //var parsedFormBody = await MultipartFormDataParser.ParseAsync(req.Body);
            //_blobService.UploadBlob(parsedFormBody.Files[0].FileName, parsedFormBody.Files[0].Data);

            var fileContent = Encoding.UTF8.GetBytes("My finance report");
            var fileContentStream = new MemoryStream(fileContent);
            var fileName = "Finance.txt";

            _blobService.UploadBlob(fileName, fileContentStream);

            var content = _blobService.DownloadTextFile(fileName);
            _logger.LogInformation($"File Content: {content}");

            var leaseId = _blobService.AcquireLeaseOnBlob(fileName);
            _logger.LogInformation($"Lease is aquired with ID: {leaseId}");

            var fileContent2 = Encoding.UTF8.GetBytes("My finance report v2");
            var fileContentStream2 = new MemoryStream(fileContent2);
            try
            {
                _blobService.UploadBlob(fileName, fileContentStream2);
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError("Blob need a lease id.");
                fileContentStream2.Position = 0;
                _blobService.UploadBlob(fileName, fileContentStream2, leaseId);

                var content2 = _blobService.DownloadTextFile(fileName);
                _logger.LogInformation($"File Content with lease: {content2}");
            }
            finally
            {
                _logger.LogInformation("Releasing lease");
                _blobService.ReleaseLeaseOnBlob(leaseId, fileName);
            }

            var token = _blobService.GetSasTokenForBlob(fileName);
            _logger.LogInformation($"SAS token URL: {token}");

            _blobService.DeleteBlob(fileName);
            _logger.LogInformation("Blob is deleted");

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
