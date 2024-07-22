using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AzureAventure6_ServiceBus
{
    public interface IServiceBusService
    {
        Task SendMessageAsync();
        Task SendMessageToTopicAsync();
        Task SendMessageToTopicWithPropertiesAsync(string category);
    }

    public class ServiceBusService : IServiceBusService
    {
        private readonly string _connectionString;
        private readonly string _queueName;
        private readonly string _topicName;
        private readonly ILogger<ServiceBusService> _logger;

        public ServiceBusService(IConfiguration configuration, ILogger<ServiceBusService> logger)
        {
            _connectionString = configuration["serviceBusConnectionString"];
            _queueName = configuration["serviceBusQueueName"];
            _topicName = configuration["serviceBusTopic"];
            _logger = logger;
        }

        public async Task SendMessageAsync()
        {
            await using var client = new ServiceBusClient(_connectionString);
            ServiceBusSender sender = client.CreateSender(_queueName);

            ServiceBusMessage message = new ServiceBusMessage("Azure Adventure!");
            await sender.SendMessageAsync(message);
            _logger.LogInformation($"Sent a single message to the queue: {_queueName}");
        }

        public async Task SendMessageToTopicAsync()
        {
            await using var client = new ServiceBusClient(_connectionString);
            ServiceBusSender sender = client.CreateSender(_topicName);

            ServiceBusMessage message = new ServiceBusMessage("Azure Adventure!");
            await sender.SendMessageAsync(message);
            _logger.LogInformation($"Sent a single message to the topic: topicName");
        }

        public async Task SendMessageToTopicWithPropertiesAsync(string category)
        {
            await using var client = new ServiceBusClient(_connectionString);
            ServiceBusSender sender = client.CreateSender(_topicName);

            ServiceBusMessage message = new ServiceBusMessage($"Adventure from {category} category");
            message.ApplicationProperties.Add("Category", category);
            await sender.SendMessageAsync(message);
            _logger.LogInformation($"Sent a single message to the topic: {_topicName}");
        }
    }
}
