using sql.builder.Clean;
using Npgsql;
using Asuse.Ai.Reports.Settings;
using Microsoft.Extensions.Options;
using System.IO;

namespace Asuse.Ai.Reports.Services
{
    public class SqlBuilderService
    {
        private readonly ILogger<SqlBuilderService> _logger;
        private readonly ReportingSettings _settings;

        public SqlBuilderService(ILogger<SqlBuilderService> logger, IOptions<ReportingSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task ExecuteReport(string reportName, string templateName, Dictionary<string, object> pars, string fileId, string fileName)
        {
           var path =  CleanSqlBuilder.ExecuteReport(reportName, templateName, pars, new Dictionary<string, object>());
           await WriteResultFile(fileId, fileName, path);
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
