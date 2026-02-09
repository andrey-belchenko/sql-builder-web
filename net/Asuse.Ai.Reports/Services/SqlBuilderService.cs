using sql.builder.Clean;

namespace Asuse.Ai.Reports.Services
{
    public class SqlBuilderService
    {
        private readonly ILogger<SqlBuilderService> _logger;

        public SqlBuilderService(ILogger<SqlBuilderService> logger)
        {
            _logger = logger;
        }

        public void ExecuteReport(string reportName, string templateName, Dictionary<string, object> pars, string fileId, string fileName)
        {
           var path =  CleanSqlBuilder.ExecuteReport(reportName, templateName, pars, new Dictionary<string, object>());
        }

        public void WriteResultFile( string fileId, string fileName, string filePath)
        {
            
        }
    }
}
