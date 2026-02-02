using Microsoft.AspNetCore.Mvc;

namespace Asuse.Ai.Reports.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExampleApiController : ControllerBase
    {
        private readonly ILogger<ExampleApiController> _logger;

        public ExampleApiController(ILogger<ExampleApiController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// GET endpoint returning simple data
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Hello from ExampleApi", timestamp = DateTime.UtcNow });
        }

        /// <summary>
        /// GET endpoint with query parameters
        /// </summary>
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string? name, [FromQuery] int? page = 1)
        {
            return Ok(new
            {
                searchTerm = name,
                page = page,
                results = new[] { "Result 1", "Result 2", "Result 3" }
            });
        }

        /// <summary>
        /// GET endpoint returning a list
        /// </summary>
        [HttpGet("items")]
        public IActionResult GetItems()
        {
            var items = new[]
            {
                new { id = 1, name = "Item 1", description = "First item" },
                new { id = 2, name = "Item 2", description = "Second item" },
                new { id = 3, name = "Item 3", description = "Third item" }
            };

            return Ok(items);
        }

        /// <summary>
        /// GET endpoint with route parameter
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(new
            {
                id = id,
                name = $"Item {id}",
                description = $"Description for item {id}",
                createdAt = DateTime.UtcNow.AddDays(-id)
            });
        }

        /// <summary>
        /// POST endpoint accepting JSON body
        /// </summary>
        [HttpPost]
        public IActionResult Create([FromBody] CreateItemRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(new { error = "Name is required" });
            }

            var newItem = new
            {
                id = new Random().Next(1000, 9999),
                name = request.Name,
                description = request.Description,
                createdAt = DateTime.UtcNow
            };

            return CreatedAtAction(nameof(GetById), new { id = newItem.id }, newItem);
        }

        /// <summary>
        /// PUT endpoint for updating
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateItemRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { error = "Request body is required" });
            }

            return Ok(new
            {
                id = id,
                name = request.Name ?? $"Updated Item {id}",
                description = request.Description,
                updatedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// DELETE endpoint
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(new { message = $"Item {id} deleted successfully", deletedAt = DateTime.UtcNow });
        }
    }

    // DTOs for request models
    public class CreateItemRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class UpdateItemRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
