using System.Text.Json;
using Azure.Messaging;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureAventure5_EventGrid
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly IEventGridService _eventGridService;

        public Function1(ILogger<Function1> logger, IEventGridService eventGridService)
        {
            _logger = logger;
            _eventGridService = eventGridService;
        }

        [Function("CloudTrigger")]
        public void CloudTrigger([EventGridTrigger] CloudEvent cloudEvent)
        {
            _logger.LogInformation($"{JsonSerializer.Serialize(cloudEvent)}");
        }        
        
        [Function("EventGrid")]
        public void EventTrigger([EventGridTrigger] EventGridEvent eventGridEvent)
        {
            _logger.LogInformation($"{JsonSerializer.Serialize(eventGridEvent)}");
        }

        [Function("SendEventGridEvent")]
        public async Task SendEventGridEvent([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            await _eventGridService.SendEventGridEventAsync();
        }
    }
}
