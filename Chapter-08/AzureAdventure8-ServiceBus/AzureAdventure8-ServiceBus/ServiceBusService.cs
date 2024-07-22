using Azure.Identity;
using Azure.Messaging.ServiceBus;

namespace AzureAdventure8_ServiceBus
{
    public interface IServiceBusService
    {
        ServiceBusClient GetClient();
    }

    public class ServiceBusService : IServiceBusService
    {
        public ServiceBusClient GetClient()
        {
            return new ServiceBusClient("busname.servicebus.windows.net", new DefaultAzureCredential());
        }
    }
}
