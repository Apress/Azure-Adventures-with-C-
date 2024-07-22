//using System.Net;
//using Microsoft.Azure.Functions.Worker;
//using Microsoft.Azure.Functions.Worker.Http;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;

//namespace AzureAdventure.FirstFunction
//{
//    public class ReturningObjectAsString
//    {
//        private readonly ILogger _logger;

//        public ReturningObjectAsString(ILoggerFactory loggerFactory)
//        {
//            _logger = loggerFactory.CreateLogger<ReturningObjectAsString>();
//        }

//        [Function("ReturningObjectAsString")]
//        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
//        {
//            _logger.LogInformation("C# HTTP trigger function processed a request.");

//            var response = req.CreateResponse(HttpStatusCode.OK);
//            response.Headers.Add("Content-Type", "application/json");

//            var objectModel = new ObjectViewModel
//            {
//                Name = "New post"
//            };
//            response.WriteString(JsonConvert.SerializeObject(objectModel));

//            return response;
//        }
//    }

//    public class ObjectViewModel
//    {
//        public string Name { get; set; }
//    }
//}
