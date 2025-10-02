using Microsoft.AspNetCore.Mvc;
using ProductService.Services;

namespace ProductService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HealthController(HealthCheckPublisher healthCheckPublisher) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHealth()
    {
        await healthCheckPublisher.PublishHealthCheckAsync();
        return Ok("Healthy");
    }
}
