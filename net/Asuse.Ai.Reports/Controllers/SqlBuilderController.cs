using Microsoft.AspNetCore.Mvc;

namespace Asuse.Ai.Reports.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SqlBuilderController : ControllerBase
    {
        private readonly ILogger<SqlBuilderController> _logger;

        public SqlBuilderController(ILogger<SqlBuilderController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// GET endpoint returning simple data
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Hello from SqlBuilderController", timestamp = DateTime.UtcNow });
        }
    }

}
