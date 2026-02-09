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

        [HttpPost("execute-report")]
        [Consumes("application/json")]
        public async Task<IActionResult> ExecuteReport(ExecuteReportRequest request)
        {
            try
            {
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
    }

}
