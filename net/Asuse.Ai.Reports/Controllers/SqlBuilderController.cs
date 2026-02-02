using Microsoft.AspNetCore.Mvc;

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
        [Route("form")]
        public IActionResult GetForm()
        {
            return Ok(new { message = "Hello from SqlBuilderController", timestamp = DateTime.UtcNow });
        }

    }

}
