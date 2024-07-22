//using System.Net;
//using Microsoft.Azure.Functions.Worker;
//using Microsoft.Azure.Functions.Worker.Http;
//using Microsoft.Extensions.Logging;

//namespace AzureAdventure.FirstFunction
//{
//    public class Function1
//    {
//        private readonly ILogger _logger;
//        //private readonly IResponseService _responseService;

//        public Function1(ILoggerFactory loggerFactory)
//        {
//            _logger = loggerFactory.CreateLogger<Function1>();
//            //_responseService = responseService;
//        }

//        [Function("Function1")]
//        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "helloworld")] HttpRequestData req)
//        {
//            _logger.LogInformation("C# HTTP trigger function processed a request.");

//            var response = req.CreateResponse(HttpStatusCode.OK);
//            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

//            //response.WriteString(_responseService.GetValue());

//            return response;
//        }
//    }
//}
