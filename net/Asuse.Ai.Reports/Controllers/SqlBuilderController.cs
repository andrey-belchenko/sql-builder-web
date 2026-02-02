using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using sql.builder.Clean;
namespace Asuse.Ai.Reports.Controllers
{
    [ApiController]
    [Route("api/sql-builder")]
    public class SqlBuilderController : ControllerBase
    {
        private readonly ILogger<SqlBuilderController> _logger;

        public SqlBuilderController(ILogger<SqlBuilderController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("form-config/{id}")]
        public IActionResult GetForm(string id)
        {
            return NotFound($"Form configuration not found for id: {id}");
            XElement xform = CleanSqlBuilder.GetFormConfig(id);
            
            if (xform == null)
            {
                return NotFound($"Form configuration not found for id: {id}");
            }

            var xmlString = xform.ToString();
            return Content(xmlString, "application/xml");
        }

    }

}
