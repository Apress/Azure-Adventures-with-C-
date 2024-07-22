using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureAdventure8_ServiceBus
{
    public class HttpBusTrigger
    {
        private readonly ILogger<HttpBusTrigger> _logger;

        public HttpBusTrigger(ILogger<HttpBusTrigger> logger)
        {
            _logger = logger;
        }

        [Function("HttpBusTrigger")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            return new OkObjectResult("");
        }
    }
}
