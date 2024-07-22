using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace AzureAventure7_SQLDatabase
{
    public class SayHiSql
    {
        private readonly ILogger<SayHiSql> _logger;
        private readonly ISqlService _sqlService;

        public SayHiSql(ILogger<SayHiSql> logger, ISqlService sqlService)
        {
            _logger = logger;
            _sqlService = sqlService;
        }

        [Function(nameof(SayHiSql))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            await _sqlService.AddBook("Azure Adventure");
            await _sqlService.AddBook("History of hello worlds");

            var books = await _sqlService.GetBooks();
            _logger.LogInformation(books);

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
