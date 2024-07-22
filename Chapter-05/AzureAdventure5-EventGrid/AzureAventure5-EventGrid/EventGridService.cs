using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.Extensions.Logging;

namespace AzureAventure5_EventGrid
{
    public interface IEventGridService
    {
        Task SendEventGridEventAsync();
    }

    public class EventGridService : IEventGridService
    {
        private readonly ILogger<EventGridService> _logger;

        public EventGridService(ILogger<EventGridService> logger)
        {
            _logger = logger;
        }

        public async Task SendEventGridEventAsync()
        {
            _logger.LogInformation("Sending event");

            var endpoint = "https://adventuresurladdressreplace.eventgrid.azure.net/api/events";
            var topicKey = "topicKey";
            var credential = new AzureKeyCredential(topicKey);
            var client = new EventGridPublisherClient(new Uri(endpoint), credential);

            await client.SendEventAsync(new EventGridEvent("MyResources", "NewResource", "1", new
            {
                Name = "Event Adventure",
                Time = DateTime.UtcNow
            }));

            _logger.LogInformation("Event sent");
        }
    }
}
