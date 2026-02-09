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

        public void ExecuteReport(string reportName, string templateName, Dictionary<string, object> pars, Dictionary<string, object> globPars)
        {
           var path =  CleanSqlBuilder.ExecuteReport(reportName, templateName, pars, globPars);
        }
    }
}
