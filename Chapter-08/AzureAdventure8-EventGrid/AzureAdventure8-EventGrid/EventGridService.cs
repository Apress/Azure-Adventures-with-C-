using Azure.Identity;
using Azure.Messaging.EventGrid;

namespace AzureAdventure8_EventGrid
{
    public class EventGridService
    {
        public EventGridPublisherClient GetPublisherClient()
        {
            return new EventGridPublisherClient(new Uri(""), new DefaultAzureCredential());
        }
    }
}
