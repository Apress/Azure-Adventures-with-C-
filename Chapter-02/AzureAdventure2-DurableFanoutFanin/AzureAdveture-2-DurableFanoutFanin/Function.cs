using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureAdveture_2_DurableFanoutFanin
{
    public static class Function
    {
        [Function(nameof(Function))]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(Function));

            var orderModels = context.GetInput<List<OrderEntity>>();
            var saveTasks = new List<Task<string>>();

            foreach (var order in orderModels)
            {
                saveTasks.Add(context.CallActivityAsync<string>(nameof(SaveMultipleOrders), order));
            }

            await Task.WhenAll(saveTasks);

            return saveTasks.Select(task => task.Result).ToList();
        }

        [Function(nameof(SaveMultipleOrders))]
        public static string SaveMultipleOrders([ActivityTrigger] OrderEntity model, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger(nameof(SaveMultipleOrders));
            model.Id = Guid.NewGuid().ToString();
            logger.LogInformation($"Saving order number: {model.Id}");

            return model.Id;
        }

        [Function("Function_HttpStart")]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("Function_HttpStart");

            var orders = req.ReadFromJsonAsync<List<OrderEntity>>();
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(Function), orders);

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return client.CreateCheckStatusResponse(req, instanceId);
        }

        public class OrderEntity
        {
            public string Id { get; set; }
            public int Quantity { get; set; }
            public string ProductName { get; set; }
            public string UserGuid { get; set; }
        }
    }
}
