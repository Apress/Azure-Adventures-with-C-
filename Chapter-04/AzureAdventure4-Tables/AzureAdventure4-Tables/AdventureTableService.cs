using Azure.Data.Tables;
using Azure;
using Microsoft.Extensions.Configuration;

namespace AzureAdventure4_Tables
{
    public class AdventureTableService : IAdventureTableService
    {
        private readonly TableClient _tableClient;

        public AdventureTableService(IConfiguration configuration)
        {
            _tableClient = CreateTableClient(configuration);
        }

        public TableClient CreateTableClient(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("AzureWebJobsStorage");
            var tableClient = new TableClient(connectionString, "reports");

            tableClient.CreateIfNotExists();

            return tableClient;
        }

        public async Task AddEntityAsync<T>(T entity) where T : ITableEntity
        {
            await _tableClient.AddEntityAsync(entity);
        }

        public async Task DeleteEntityAsync(string partitionKey, string rowKey)
        {
            await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        public async Task UpdateEntityAsync<T>(T entity) where T : ITableEntity
        {
            await _tableClient.UpdateEntityAsync(entity, ETag.All);
        }

        public async Task<T?> GetEntityAsync<T>(string partitionKey, string rowKey) where T : class, ITableEntity, new()
        {
            try
            {
                return await _tableClient.GetEntityAsync<T>(partitionKey, rowKey);
            }
            catch (RequestFailedException ex)
            {
                if (ex.Status == 404)
                {
                    Console.WriteLine("Entity not found.");
                }

                return null;
            }
        }

        public List<T> Query<T>(string query) where T : class, ITableEntity, new()
        {
            Pageable<T> entities = _tableClient.Query<T>(query);

            return entities.ToList();
        }
    }
}
