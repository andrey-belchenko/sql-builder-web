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
            XElement xform = CleanSqlBuilder.GetFormConfig(id);
            var dummyXml = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<form name=""{id}"">
    <fields>
        <field name=""field1"" type=""text"" label=""Field 1"" />
        <field name=""field2"" type=""number"" label=""Field 2"" />
    </fields>
</form>";

            return Content(dummyXml, "application/xml");
        }

    }

}
