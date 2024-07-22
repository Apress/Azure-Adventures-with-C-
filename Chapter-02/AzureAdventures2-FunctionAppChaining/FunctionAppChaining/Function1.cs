using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionAppChaining
{
    public static class Function1
    {
        [Function(nameof(Function1))]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(Function1));
            logger.LogInformation("Starting Orchestrator...");
            var results = new List<string>();
            var inputEntity = context.GetInput<OrderEntity>();

            var entity = await context.CallActivityAsync<OrderEntity>(nameof(SavingDataToDb), inputEntity);

            results.Add(await context.CallActivityAsync<string>(nameof(SendingNotification), entity));

            return results;
        }

        [Function(nameof(SavingDataToDb))]
        public static OrderEntity SavingDataToDb([ActivityTrigger] OrderEntity entity, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger(nameof(SavingDataToDb));
            entity.Id = 1;
            logger.LogInformation($"Saving entity to DB: {JsonConvert.SerializeObject(entity)}");

            return entity;
        }

        [Function(nameof(SendingNotification))]
        public static string SendingNotification([ActivityTrigger] OrderEntity dbEntity, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger(nameof(SendingNotification));
            logger.LogInformation("Sending notification....");

            return dbEntity.Id.ToString();
        }

        [Function("Function1_HttpStart")]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("Function1_HttpStart");

            var data = await req.ReadFromJsonAsync<OrderEntity>();
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(Function1), data);

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return client.CreateCheckStatusResponse(req, instanceId);
        }
    }

    public class OrderEntity
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string UserGuid { get; set; }
    }
}
