using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureAdventure4_Blobs
{
    public class BlobTriggerFunction
    {
        //private readonly ILogger<BlobTriggerFunction> _logger;

        //public BlobTriggerFunction(ILogger<BlobTriggerFunction> logger)
        //{
        //    _logger = logger;
        //}

        //[Function(nameof(BlobTriggerFunction))]
        //public async Task Run([BlobTrigger("reports/{name}", Connection = "AzureWebJobsStorage")] Stream stream, string name)
        //{
        //    using var blobStreamReader = new StreamReader(stream);
        //    var content = await blobStreamReader.ReadToEndAsync();
        //    _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {content}");
        //}
    }
}
