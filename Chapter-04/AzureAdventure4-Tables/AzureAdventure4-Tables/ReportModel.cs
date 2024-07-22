using Azure;
using Azure.Data.Tables;

namespace AzureAdventure4_Tables
{
    public class ReportModel : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string FileName { get; set; }
    }
}
