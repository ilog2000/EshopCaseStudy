using System;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductService.Controllers;
using ProductService.DTOs;
using ProductService.Services;

namespace ProductServiceTests;

public class ProductControllerTests
{
    Mock<MassTransit.IPublishEndpoint> mockPublishEndpoint = new();

    public ProductControllerTests()
    {
        mockPublishEndpoint
            .Setup(pe => pe.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((message, _) =>
            {
                var json = JsonSerializer.Serialize(message);
                Debug.WriteLine($"Published message: {json}");
            })
            .Returns(Task.CompletedTask);
    }

    [Fact]
    public async Task GetProducts_ReturnsOkResult_WithProducts()
    {
        // Arrange
        var inStock = false;
        var mockService = new Mock<IProductDomainService>();
        mockService.Setup(s => s.GetAllAsync(inStock)).ReturnsAsync(new List<ProductDto>
        {
            new ProductDto { Id = Guid.NewGuid(), Name = "Product 1" },
            new ProductDto { Id = Guid.NewGuid(), Name = "Product 2" }
        });
        var controller = new ProductController(mockService.Object, mockPublishEndpoint.Object);

        // Act
        var result = await controller.GetProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var products = Assert.IsAssignableFrom<List<ProductDto>>(okResult.Value);
        Assert.Equal(2, products.Count);
        Assert.Equal("Product 1", products[0].Name);
        Assert.Equal("Product 2", products[1].Name);
    }

    [Fact]
    public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var mockService = new Mock<IProductDomainService>();
        mockService.Setup(s => s.GetByIdAsync(productId)).ReturnsAsync((ProductDto?)null);
        var controller = new ProductController(mockService.Object, mockPublishEndpoint.Object);

        // Act
        var result = await controller.GetProduct(productId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreatedAtActionResult_WithCreatedProduct()
    {
        // Arrange
        var createDto = new CreateProductDto
        {
            Name = "New Product",
            Description = "New Product Description",
            Price = 99.99m,
            StockQuantity = 10,
            CategoryIds = new List<Guid> { Guid.NewGuid() }
        };
        var createdProduct = new ProductDto
        {
            Id = Guid.NewGuid(),
            Name = createDto.Name,
            Description = createDto.Description,
            Price = createDto.Price,
            StockQuantity = createDto.StockQuantity,
        };
        var mockService = new Mock<IProductDomainService>();
        mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(createdProduct);
        var controller = new ProductController(mockService.Object, mockPublishEndpoint.Object);

        // Act
        var result = await controller.CreateProduct(createDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<ProductDto>(createdAtActionResult.Value);
        Assert.Equal(createdProduct.Id, returnValue.Id);
        Assert.Equal("New Product", returnValue.Name);
        Assert.Equal("New Product Description", returnValue.Description);
        Assert.Equal(99.99m, returnValue.Price);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsOkResult_WithUpdatedProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var updateDto = new UpdateProductDto
        {
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 79.99m,
            StockQuantity = 5,
            CategoryIds = new List<Guid> { Guid.NewGuid() }
        };
        var updatedProduct = new ProductDto
        {
            Id = productId,
            Name = updateDto.Name,
            Description = updateDto.Description,
            Price = updateDto.Price ?? 0,
            StockQuantity = updateDto.StockQuantity ?? 0,
        };
        var mockService = new Mock<IProductDomainService>();
        mockService.Setup(s => s.UpdateAsync(productId, updateDto)).ReturnsAsync(updatedProduct);
        var controller = new ProductController(mockService.Object, mockPublishEndpoint.Object);

        // Act
        var result = await controller.UpdateProduct(productId, updateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ProductDto>(okResult.Value);
        Assert.Equal(updatedProduct.Id, returnValue.Id);
        Assert.Equal("Updated Product", returnValue.Name);
        Assert.Equal("Updated Description", returnValue.Description);
        Assert.Equal(79.99m, returnValue.Price);
    }

    [Fact]
    public async Task UpdateProductStock_ReturnsOkResult_WithUpdatedProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var newStockQuantity = 20;
        var updatedProduct = new ProductDto
        {
            Id = productId,
            Name = "Product with Updated Stock",
            Description = "Description",
            Price = 49.99m,
            StockQuantity = newStockQuantity,
        };
        var mockService = new Mock<IProductDomainService>();
        mockService.Setup(s => s.UpdateProductStockAsync(productId, newStockQuantity)).ReturnsAsync(updatedProduct);
        var controller = new ProductController(mockService.Object, mockPublishEndpoint.Object);

        // Act
        var result = await controller.UpdateProductStock(productId, newStockQuantity);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ProductDto>(okResult.Value);
        Assert.Equal(updatedProduct.Id, returnValue.Id);
        Assert.Equal("Product with Updated Stock", returnValue.Name);
        Assert.Equal(20, returnValue.StockQuantity);
    }

    [Fact]
    public async Task UpdateProductStock_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var newStockQuantity = 15;
        var mockService = new Mock<IProductDomainService>();
        mockService.Setup(s => s.UpdateProductStockAsync(productId, newStockQuantity)).ReturnsAsync((ProductDto?)null);
        var controller = new ProductController(mockService.Object, mockPublishEndpoint.Object);

        // Act
        var result = await controller.UpdateProductStock(productId, newStockQuantity);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetProduct_ReturnsOkResult_WithProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new ProductDto
        {
            Id = productId,
            Name = "Existing Product",
            Description = "Existing Product Description",
            Price = 59.99m,
            StockQuantity = 8,
        };
        var mockService = new Mock<IProductDomainService>();
        mockService.Setup(s => s.GetByIdAsync(productId)).ReturnsAsync(product);
        var controller = new ProductController(mockService.Object, mockPublishEndpoint.Object);

        // Act
        var result = await controller.GetProduct(productId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ProductDto>(okResult.Value);
        Assert.Equal(product.Id, returnValue.Id);
        Assert.Equal("Existing Product", returnValue.Name);
        Assert.Equal("Existing Product Description", returnValue.Description);
        Assert.Equal(59.99m, returnValue.Price);
    }
}
