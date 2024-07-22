using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureAdventure8_EventGrid
{
    public class HttpGridTrigger
    {
        private readonly ILogger<HttpGridTrigger> _logger;

        public HttpGridTrigger(ILogger<HttpGridTrigger> logger)
        {
            _logger = logger;
        }

        [Function("HttpGridTrigger")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            return new OkObjectResult("");
        }
    }
}
