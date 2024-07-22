using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace AzureAdventure8_Storage
{
    public class HttpStorageTrigger
    {
        [Function("Function1")]
        public void Run([QueueTrigger("incoming-reports", Connection = "AzureAdventure")] string message)
        {
        }
    }
}
