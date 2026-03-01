using System.Data;
using System.Xml.Linq;
using Asuse.Ai.Reports.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using sql.builder.DataApi;

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

        public async Task SaveDataSet(VDataSet dataSet, string dataSetId)
        {
            var mongoDb = _mongoClient.GetDatabase(_settings.MongoTempDb);
            var infoCollectionName = dataSetId + ":info";
            var infoCollection = mongoDb.GetCollection<BsonDocument>(infoCollectionName);
            XElement scheme = dataSet.Scheme;

            // Delete existing info documents
            await infoCollection.DeleteManyAsync(FilterDefinition<BsonDocument>.Empty);

            var infoDoc = new BsonDocument
                {
                    { "multiple", true },
                    { "tables", new BsonDocument() }
                };
            var tablesDoc = infoDoc["tables"].AsBsonDocument;

            if (scheme != null)
            {
                infoDoc["scheme"] = ConvertXElementToBsonDocument(scheme);
            }

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

        private static BsonDocument ConvertXElementToBsonDocument(XElement element)
        {
            if (element == null)
                return new BsonDocument();

            var doc = new BsonDocument { ["_tag"] = element.Name.LocalName };

            foreach (var attr in element.Attributes())
            {
                doc[attr.Name.LocalName] = attr.Value;
            }

            var children = element.Elements().ToList();
            if (children.Count > 0)
            {
                doc["_children"] = new BsonArray(children.Select(c => (BsonValue)ConvertXElementToBsonDocument(c)));
            }

            return doc;
        }
    }
}
