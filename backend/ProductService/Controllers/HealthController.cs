using System.Threading.Tasks;
using Core.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HealthController(IPublishEndpoint publishEndpoint) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHealth()
    {
        await publishEndpoint.Publish(new HealthCheck());
        return Ok("Healthy");
    }
}
