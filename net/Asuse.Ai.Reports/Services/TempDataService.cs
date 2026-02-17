using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
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

        public async Task SaveDataSet(DataSet dataSet, string dataSetId)
        {
            var mongoDb = _mongoClient.GetDatabase(_settings.MongoTempDb);
            var infoCollectionName = dataSetId + ":info";
            var infoCollection = mongoDb.GetCollection<BsonDocument>(infoCollectionName);

            // Delete existing info documents
            await infoCollection.DeleteManyAsync(FilterDefinition<BsonDocument>.Empty);

            if (dataSet.Tables.Count == 1)
            {
                // Single table case
                var infoDoc = new BsonDocument { { "multiple", false } };
                await infoCollection.InsertOneAsync(infoDoc);

                var collection = mongoDb.GetCollection<BsonDocument>(dataSetId);
                await collection.DeleteManyAsync(FilterDefinition<BsonDocument>.Empty);

                var table = dataSet.Tables[0];
                if (table.Rows.Count > 0)
                {
                    var documents = ConvertDataTableToBsonDocuments(table);
                    await collection.InsertManyAsync(documents);
                }
            }
            else
            {
                // Multiple tables case
                var infoDoc = new BsonDocument
                {
                    { "multiple", true },
                    { "tables", new BsonDocument() }
                };
                var tablesDoc = infoDoc["tables"].AsBsonDocument;

                foreach (DataTable table in dataSet.Tables)
                {
                    tablesDoc[table.TableName] = true;

                    var collectionName = dataSetId + "." + table.TableName;
                    var collection = mongoDb.GetCollection<BsonDocument>(collectionName);
                    await collection.DeleteManyAsync(FilterDefinition<BsonDocument>.Empty);

                    if (table.Rows.Count > 0)
                    {
                        var documents = ConvertDataTableToBsonDocuments(table);
                        await collection.InsertManyAsync(documents);
                    }
                }

                await infoCollection.InsertOneAsync(infoDoc);
            }
        }

        private List<BsonDocument> ConvertDataTableToBsonDocuments(DataTable table)
        {
            var documents = new List<BsonDocument>();

            foreach (DataRow row in table.Rows)
            {
                var doc = new BsonDocument();
                foreach (DataColumn column in table.Columns)
                {
                    var value = row[column.ColumnName];
                    if (value == DBNull.Value || value == null)
                    {
                        doc[column.ColumnName] = BsonNull.Value;
                    }
                    else
                    {
                        var val = value;
                        if (val is decimal d)
                            val = (double)d;
                        doc[column.ColumnName] = BsonValue.Create(val);
                    }
                }
                documents.Add(doc);
            }

            return documents;
        }
    }
}
