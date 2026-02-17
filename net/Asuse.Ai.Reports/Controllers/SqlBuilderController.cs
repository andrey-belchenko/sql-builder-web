using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using sql.builder.Clean;
using Asuse.Ai.Reports.Services;
using Asuse.Ai.Reports.Models;

namespace Asuse.Ai.Reports.Controllers
{
    [ApiController]
    [Route("api/sql-builder")]
    public class SqlBuilderController : ControllerBase
    {
        private readonly ILogger<SqlBuilderController> _logger;
        private readonly SqlBuilderService _sqlBuilderService;

        public SqlBuilderController(ILogger<SqlBuilderController> logger, SqlBuilderService sqlBuilderService)
        {
            _logger = logger;
            _sqlBuilderService = sqlBuilderService;
        }

        [HttpPost("execute-excel-report")]
        [Consumes("application/json")]
        public async Task<IActionResult> ExecuteExcelReport(ExecuteExcelReportRequest request)
        {
            try
            {

                PrepareParams(request.Parameters);
                await _sqlBuilderService.ExecuteReport(
                    request.ReportName,
                    request.TemplateName,
                    request.Parameters,
                    request.FileId,
                    request.FileName
                );

                return Ok(new { message = "Report executed successfully", fileId = request.FileId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing report: {ReportName}", request.ReportName);
                return StatusCode(500, new { error = "An error occurred while executing the report", message = ex.Message });
            }
        }

        [HttpPost("execute-report")]
        [Consumes("application/json")]
        public async Task<IActionResult> ExecuteReport(ExecuteReportRequest request)
        {
            try
            {
                PrepareParams(request.Parameters);
                await _sqlBuilderService.ExecuteReport(
                    request.ReportName,
                    request.Parameters,
                    request.DataSetId
                );

                return Ok(new { message = "Report executed successfully", dataSetId = request.DataSetId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing report: {ReportName}", request.ReportName);
                return StatusCode(500, new { error = "An error occurred while executing the report", message = ex.Message });
            }
        }

        private static void PrepareParams(Dictionary<string, object> pars)
        {
            foreach (var it in pars.ToArray())
            {
                if (it.Value is bool)
                {
                    pars.Remove(it.Key);
                    var val = (bool)it.Value;
                    if (val)
                    {
                        pars.Add(it.Key, 1m);
                    }
                    else
                    {
                        pars.Add(it.Key, 0m);
                    }
                }
            }
        }
    }

}
