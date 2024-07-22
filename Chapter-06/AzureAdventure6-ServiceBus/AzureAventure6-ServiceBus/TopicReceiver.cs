using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace AzureAventure6_ServiceBus
{
    public class TopicReceiver
    {
        private readonly ILogger<TopicReceiver> _logger;
        private readonly IServiceBusService _serviceBusService;

        public TopicReceiver(ILogger<TopicReceiver> logger, IServiceBusService serviceBusService)
        {
            _logger = logger;
            _serviceBusService = serviceBusService;
        }

        [Function(nameof(TopicReceiver))]
        public async Task Run(
            [ServiceBusTrigger("%serviceBusTopic%", "%serviceBusSubscription%", Connection = "serviceBusConnectionString")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation($"Received message {message.Body}");
            await messageActions.CompleteMessageAsync(message);
        }

        [Function(nameof(SendMessageToTopic))]
        public async Task SendMessageToTopic([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
                       FunctionContext context)
        {
            await _serviceBusService.SendMessageToTopicWithPropertiesAsync("Adventure");
            await _serviceBusService.SendMessageToTopicWithPropertiesAsync("Walk");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await response.WriteStringAsync("Message sent to topic");
        }
    }
}
