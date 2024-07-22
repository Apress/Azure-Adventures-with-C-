//using System.Net;
//using Microsoft.Azure.Functions.Worker;
//using Microsoft.Azure.Functions.Worker.Http;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;

//namespace AzureAdventure.FirstFunction
//{
//    public class ReadingFromRequest
//    {
//        private readonly ILogger _logger;

//        public ReadingFromRequest(ILoggerFactory loggerFactory)
//        {
//            _logger = loggerFactory.CreateLogger<ReadingFromRequest>();
//        }

//        [Function("ReadingFromRequest")]
//        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "ReadingFromRequest/{name}")] HttpRequestData req, string name)
//        {
//            _logger.LogInformation("C# HTTP trigger function processed a request.");

//            _logger.LogInformation(name);

//            var response = req.CreateResponse(HttpStatusCode.OK);
//            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

//            response.WriteString("Welcome to Azure Functions!");

//            return response;
//        }
//    }
//}
