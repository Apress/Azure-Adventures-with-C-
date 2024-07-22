using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace AzureAdventure_Middleware
{
    public class CustomMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            // read parameter from the path
            context.BindingContext.BindingData.TryGetValue("name", out var name);

            var data = await context.GetHttpRequestDataAsync();
            // get headers
            var auhtorizationHeader = data.Headers.FirstOrDefault(h => h.Key.Equals("Authorization"));

            // get body
            var body = await data.ReadAsStringAsync();

            // get parameters from query
            var id1 = data.Query.GetValues("id");

            // get parameters from path

            // get claims
            var name2 = data.Identities.FirstOrDefault()?.NameClaimType;
            await next.Invoke(context);
        }
    }
}
