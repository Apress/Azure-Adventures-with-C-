using Microsoft.Extensions.Logging;

namespace AzureAdventure3_AppInsights
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
            _logger.LogTrace("Trace");
            _logger.LogDebug("Debug");
            _logger.LogInformation("Information");
            _logger.LogWarning("Warning");
            _logger.LogError("Error");
            _logger.LogCritical("Critical");
            _logger.LogMetric("Custom Metric", 2.0);
            _logger.LogMetric("Custom Metric", 1.0, new Dictionary<string, object>()
            {
                {
                    "CustomProperty", "CustomValue"
                }
            });

            using (_logger.BeginScope("Scope"))
            {
                _logger.LogInformation("Information in scope");
                _logger.LogInformation("Second information in scope");
            }

            return "Hi!";
        }
    }

    public interface IResponseService
    {
        string GetValue();
    }

}
