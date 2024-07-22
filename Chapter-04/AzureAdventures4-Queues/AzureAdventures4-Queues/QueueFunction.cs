using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace AzureAdventures4_Queues
{
    public class QueueFunction
    {
        private readonly IQueuesService _queuesService;

        public QueueFunction(IQueuesService queuesService)
        {
            _queuesService = queuesService;
        }

        [Function("QueueFunction")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _queuesService.SendMessage("Test Message");
            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
