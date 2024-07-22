using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AzureAventure7_SQLDatabase
{
    public interface ISqlService
    {
        Task AddBook(string title);
        Task<string> GetBooks();
    }

    public class SqlService : ISqlService
    {
        private readonly string _connectionString;
        private readonly ILogger<SqlService> _logger;

        public SqlService(IConfiguration configuration, ILogger<SqlService> logger)
        {
            _connectionString = configuration["SQLConnectionString"];
            _logger = logger;
        }

        public async Task AddBook(string title)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("INSERT INTO Book (Title) VALUES (@title)", connection);
            command.Parameters.AddWithValue("@title", title);

            await command.ExecuteNonQueryAsync();

            _logger.LogInformation("Added book to the database");
        }

        public async Task<string> GetBooks()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("SELECT Title FROM Book", connection);
            var stringBuilder = new StringBuilder();
            var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                stringBuilder.Append(reader["Title"]);
                stringBuilder.Append(", ");
            }

            return stringBuilder.ToString();
        }
    }
}
