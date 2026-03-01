using Asuse.Ai.Reports.Settings;
using Microsoft.Extensions.Options;
using Npgsql;
using sql.builder.Clean;

namespace Asuse.Ai.Reports.Services
{
    public class SqlBuilderService
    {
        private const string DefaultOracleConnectionString = "User Id=asuse;Password=kl0pik;Server=realryaz;Pooling=False;Sid=realryaz;Port=1521";

        private readonly ILogger<SqlBuilderService> _logger;
        private readonly ReportingSettings _settings;
        private readonly TempDataService _tempDataService;

        public SqlBuilderService(ILogger<SqlBuilderService> logger, IOptions<ReportingSettings> settings, TempDataService tempDataService)
        {
            _logger = logger;
            _settings = settings.Value;
            _tempDataService = tempDataService;
        }

        public async Task ExecuteReport(string reportName, string templateName, Dictionary<string, object> pars, string fileId, string fileName)
        {
            var connectionString = _settings.OracleConnectionString ?? DefaultOracleConnectionString;
            using var conn = new VOracleConnection(connectionString);
            conn.Open();

            var path = CleanSqlBuilder.ExecuteReport(reportName, templateName, pars, new Dictionary<string, object>(), connection: conn);
            await WriteResultFile(fileId, fileName, path);
        }

        public async Task ExecuteReport(string reportName, Dictionary<string, object> pars, string datasetId)
        {
            var connectionString = _settings.OracleConnectionString ?? DefaultOracleConnectionString;
            using var conn = new VOracleConnection(connectionString);
            conn.Open();
            var ds = CleanSqlBuilder.ExecuteReportGetDs(reportName, pars, new Dictionary<string, object>(), connection: conn);
            await _tempDataService.SaveDataSet(ds, datasetId);
        }

        public async Task WriteResultFile(string fileId, string fileName, string filePath)
        {
            var fileBytes = await File.ReadAllBytesAsync(filePath);

            await using var conn = new NpgsqlConnection(_settings.PgConnectionString);
            await conn.OpenAsync();
            var sql = "INSERT INTO report_sys.file (file_id, file_name, file_data) VALUES (@fileId, @fileName, @fileData) ON CONFLICT (file_id) DO UPDATE SET file_name = @fileName, file_data = @fileData, changed_at = CURRENT_TIMESTAMP";
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("fileId", fileId);
            cmd.Parameters.AddWithValue("fileName", fileName);
            cmd.Parameters.AddWithValue("fileData", fileBytes);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
