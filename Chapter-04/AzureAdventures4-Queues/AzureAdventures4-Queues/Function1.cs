using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureAdventures4_Queues
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        //[Function(nameof(Function1))]
        //public void Run([QueueTrigger("incoming-reports")] QueueMessage message)
        //{
        //    _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        //}


        //[Function(nameof(Function1))]
        //public void Run([QueueTrigger("incoming-reports2")] string message)
        //{
        //    _logger.LogInformation($"New message in the queue: {message}");
        //}


        [Function(nameof(Function1))]
        public void Run([QueueTrigger("incoming-reports")] ReportModel message)
        {
            _logger.LogInformation($"Report Name: {message.Name}, ID: {message.Id}");
        }
    }

    public class ReportModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
