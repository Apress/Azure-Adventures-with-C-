using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace AzureAdventure2_MonitoringPattern
{
    public static class Function1
    {
        [Function(nameof(Function1))]
        public static async Task<string> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(Function1));

            using var canncelationToken = new CancellationTokenSource();

            var expiryTime = TimeSpan.FromMinutes(20);

            var expiryTask = context.CreateTimer(context.CurrentUtcDateTime.Add(expiryTime), canncelationToken.Token);
            var resultTask = context.CallActivityAsync<string>(nameof(LongRunningTask), "Inputs");
            await Task.WhenAny(resultTask, expiryTask);

            if (resultTask.IsCompletedSuccessfully)
            {
                return "Task completed successfully";
            }

            return "Timeout!";
        }

        [Function(nameof(LongRunningTask))]
        public static string LongRunningTask([ActivityTrigger] string name, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger(nameof(LongRunningTask));

            Task.Delay(TimeSpan.FromMinutes(1));

            return "Completed";
        }

        [Function("Function1_HttpStart")]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("Function1_HttpStart");

            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(Function1));

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return client.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
