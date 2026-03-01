using System.Data;
using Asuse.Ai.Reports.Services;
using FastReport.Web;
using Microsoft.AspNetCore.Mvc;
namespace Asuse.Ai.Reports.Controllers
{
    public class ReportController : Controller
    {
        private readonly ILogger<ReportController> _logger;
        private IWebHostEnvironment _env;
        private ReportingService _reportingService;
        public ReportController(ILogger<ReportController> logger, IWebHostEnvironment env, ReportingService reportingService)
        {
            _logger = logger;
            _env = env;
            _reportingService = reportingService;
        }


        public async Task<IActionResult> DisplayReport(string dataSetName, string templateId, bool? singleTable)
        {

            var webReport = await _reportingService.PrepareReport(dataSetName, templateId, singleTable ?? false);
            ViewBag.WebReport = webReport;
            return View("report");
        }

        public IActionResult Example()
        {
            var webRoot = _env.WebRootPath;
            WebReport webReport = new WebReport();
            webReport.Width = "1000";
            webReport.Height = "1000";
            webReport.Report.Load(Path.Combine(webRoot, "templates/example.frx"));
            var table = new DataTable();
            table.Columns.Add("value");
            table.Rows.Add(1);
            table.Rows.Add(2);
            table.Rows.Add(3);
            webReport.Report.RegisterData(table, "my_table");
            ViewBag.WebReport = webReport;
            return View("report");
        }




    }
}
