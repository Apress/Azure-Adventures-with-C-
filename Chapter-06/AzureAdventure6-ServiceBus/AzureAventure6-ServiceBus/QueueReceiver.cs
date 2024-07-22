using System.Net;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureAventure6_ServiceBus
{
    public class QueueReceiver
    {
        private readonly ILogger<QueueReceiver> _logger;
        private readonly IServiceBusService _serviceBusService;

        public QueueReceiver(ILogger<QueueReceiver> logger, IServiceBusService serviceBusService)
        {
            _logger = logger;
            _serviceBusService = serviceBusService;
        }

        [Function(nameof(QueueReceiver))]
        public async Task Run(
            [ServiceBusTrigger("%serviceBusQueueName%", Connection = "serviceBusConnectionString")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation($"Received message: {message.Body.ToString()}");
            await messageActions.CompleteMessageAsync(message);
        }

        [Function(nameof(SendMessage))]
        public async Task SendMessage([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            await _serviceBusService.SendMessageAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await response.WriteStringAsync("Message sent to queue");
        }
    }
}
