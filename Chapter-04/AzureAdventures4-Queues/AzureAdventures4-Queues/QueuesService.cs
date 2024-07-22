using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AzureAdventures4_Queues
{
    public class QueuesService : IQueuesService
    {
        private readonly IConfiguration _configuration;

        public QueuesService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMessage(string message)
        {
            var connectionString = _configuration.GetValue<string>("AzureWebJobsStorage");
            var queueClient = new QueueClient(connectionString, "incoming-reports",new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            queueClient.SendMessage(message);
        }

        public void SendObjectMessage(ReportModel model)
        {
            SendMessage(JsonConvert.SerializeObject(model));
        }
    }
}
