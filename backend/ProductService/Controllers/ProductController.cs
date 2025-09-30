using Core.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ProductService.DTOs;
using ProductService.Mapping;
using ProductService.Services;

namespace ProductService.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductController(IProductDomainService productDomainServce, IPublishEndpoint publishEndpoint) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<List<ProductDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts([FromQuery] bool inStock = false)
    {
        var products = await productDomainServce.GetAllAsync(inStock);
        return Ok(products);
    }

    [HttpGet("{id}")]
    [ProducesResponseType<ProductDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var product = await productDomainServce.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    [ProducesResponseType<ProductDto>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
    {
        var product = await productDomainServce.CreateAsync(dto);
        await publishEndpoint.Publish(product.ToProductCreatedMessage());
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    [ProducesResponseType<ProductDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto updatedProduct)
    {
        var product = await productDomainServce.UpdateAsync(id, updatedProduct);
        if (product == null)
        {
            return NotFound();
        }
        await publishEndpoint.Publish(product.ToProductUpdatedMessage());
        return Ok(product);
    }

    [HttpPatch("{id}/stock")]
    [ProducesResponseType<ProductDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProductStock(Guid id, [FromBody] int newStockQuantity)
    {
        var product = await productDomainServce.UpdateProductStockAsync(id, newStockQuantity);
        if (product == null)
        {
            return NotFound();
        }
        await publishEndpoint.Publish(product.ToProductUpdatedMessage());
        return Ok(product);
    }

    // TODO: Decide to use hard delete or soft delete
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var success = await productDomainServce.DeleteAsync(id);
        if (!success)
        {
            return NotFound();
        }
        await publishEndpoint.Publish(new ProductDeleted { Id = id });
        return NoContent();
    }
}
