using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureAdventure4_Tables
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly IAdventureTableService _tableService;

        public Function1(ILoggerFactory loggerFactory, IAdventureTableService tableService)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _tableService = tableService;
        }

        [Function("Function1")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            var entity = new ReportModel()
            {
                PartitionKey = "partition1",
                RowKey = "row1",
                FileName = "SecretReport.xls",
            };

            await _tableService.AddEntityAsync(entity);

            var retrievedEntity = await _tableService.GetEntityAsync<ReportModel>("partition1", "row1");

            if (retrievedEntity != null)
            {
                _logger.LogInformation($"Retrieved entity: PartitionKey={retrievedEntity.PartitionKey}, RowKey={retrievedEntity.RowKey}, FileName={retrievedEntity.FileName}");

                retrievedEntity.FileName = "IWillNeverGiveYouUp.xls";
                await _tableService.UpdateEntityAsync(retrievedEntity);
                _logger.LogInformation($"Updated entity: PartitionKey={retrievedEntity.PartitionKey}, RowKey={retrievedEntity.RowKey}, FileName= {retrievedEntity.FileName}");

                var entities = _tableService.Query<ReportModel>("PartitionKey eq 'partition1'");
                _logger.LogInformation($"Queried entities: {entities.Count}");

                await _tableService.DeleteEntityAsync("partition1", "row1");
                _logger.LogInformation("Deleted entity.");
            }

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
