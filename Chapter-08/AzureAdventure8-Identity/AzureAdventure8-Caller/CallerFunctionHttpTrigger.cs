using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;

namespace AzureAdventure8_Caller
{
    public class CallerFunctionHttpTrigger
    {
        private readonly IGetDataService _getDataService;

        public CallerFunctionHttpTrigger(IGetDataService getDataService)
        {
            _getDataService = getDataService;
        }

        [Function("Caller")]
        public async Task Caller([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            await _getDataService.GetDataAsync();
        }
    }
}
