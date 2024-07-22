using Microsoft.Extensions.Logging;

namespace AzureAdventure.FirstFunction
{
    public class ResponseService : IResponseService
    {
        private readonly ILogger<ResponseService> _logger;

        public ResponseService(ILogger<ResponseService> logger)
        {
            _logger = logger;
        }

        public string GetValue()
        {
            _logger.LogInformation("Running GetValue");

            return "Hi!";
        }
    }

    public interface IResponseService
    {
        string GetValue();
    }
}
