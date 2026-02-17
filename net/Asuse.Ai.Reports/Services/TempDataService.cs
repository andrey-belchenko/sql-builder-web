using MongoDB.Bson;
using MongoDB.Driver;
using System.Data;
using Asuse.Ai.Reports.Settings;
using Microsoft.Extensions.Options;

namespace Asuse.Ai.Reports.Services
{
    public class TempDataService
    {
        private readonly ILogger<TempDataService> _logger;
        private readonly ReportingSettings _settings;
        private readonly MongoClient _mongoClient;

        public TempDataService(ILogger<TempDataService> logger, IOptions<ReportingSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
            _mongoClient = new MongoClient(_settings.MongoConnectionString);
        }

        public async Task FillDataSet(DataSet dataSet, string tempDataSetName, bool isSingleTable)
        {
            var mongoDb = _mongoClient.GetDatabase(_settings.MongoTempDb);
            var collection = mongoDb.GetCollection<BsonDocument>(tempDataSetName);

            foreach (DataTable table in dataSet.Tables)
            {
                var filter = new BsonDocument();
                if (!isSingleTable)
                {
                    filter = Builders<BsonDocument>.Filter.Exists(table.TableName).ToBsonDocument();
                }
                using var cursor = await collection.FindAsync(filter);
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        var item = document;
                        if (!isSingleTable)
                        {
                            item = (BsonDocument)document[table.TableName];
                        }
                        DataRow row = table.NewRow();
                        foreach (DataColumn column in table.Columns)
                        {
                            object value = DBNull.Value;
                            if (item.Contains(column.ColumnName))
                            {
                                value = BsonTypeMapper.MapToDotNetValue(item[column.ColumnName]) ?? DBNull.Value;
                            }
                            row[column.ColumnName] = value;
                        }
                        table.Rows.Add(row);
                    }
                }
            }
        }
    }
}
