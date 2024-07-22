using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;

namespace AzureAdventure4_Tables
{
    public interface IAdventureTableService
    {
        Task AddEntityAsync<T>(T entity) where T : ITableEntity;
        TableClient CreateTableClient(IConfiguration configuration);
        Task DeleteEntityAsync(string partitionKey, string rowKey);
        Task<T?> GetEntityAsync<T>(string partitionKey, string rowKey) where T : class, ITableEntity, new();
        List<T> Query<T>(string query) where T : class, ITableEntity, new();
        Task UpdateEntityAsync<T>(T entity) where T : ITableEntity;
    }
}