using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Logging;

namespace AzureAdventure8_Caller
{
    public interface IGetDataService
    {
        Task<string> GetDataAsync();
    }

    public class GetDataService : IGetDataService
    {
        private readonly ILogger<GetDataService> _logger;

        public GetDataService(ILogger<GetDataService> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetDataAsync()
        {
            try
            {
                var token = GetAccessToken();

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var response = await httpClient.GetAsync("httpFunctionApiUrl");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Function response: {content}");
                }
                else
                {
                    _logger.LogError($"Error calling the function. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
            }

            return "";
        }

        private string GetAccessToken()
        {
            var credential = new DefaultAzureCredential();

            var resource = "api://<function-api-guid>/.default";
            var token = credential.GetToken(new TokenRequestContext(new[] { resource }));

            return token.Token;
        }
    }
}
