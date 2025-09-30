using Microsoft.AspNetCore.Mvc;
using ProductService.DTOs;
using ProductService.Services;

namespace ProductService.Controllers;

[Route("api/v2/[controller]")]
[ApiController]
public class ProductV2Controller(IProductDomainService productDomainServce) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<List<ProductDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts([FromQuery] bool inStock = false, [FromQuery] int pageNr = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNr < 1 || pageSize < 1)
        {
            return BadRequest("PageNumber and PageSize must be greater than 0.");
        }

        var products = await productDomainServce.GetPagedAsync(inStock, pageNr, pageSize);
        return Ok(products);
    }
}
