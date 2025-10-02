using System.Diagnostics;
using System.Text.Json;
using Core.Builders;
using Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProductService.Controllers;
using ProductService.DataAccess;
using ProductService.DataAccess.Repositories;
using ProductService.DTOs;
using ProductService.Services;

namespace ProductServiceTests;

public class ProductV2ControllerTests
{
    Mock<MassTransit.IPublishEndpoint> mockPublishEndpoint = new();

    public ProductV2ControllerTests()
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
    public async Task GetProducts_ReturnsOkResult_WithPagedProducts()
    {
        // Arrange
        var products = new List<Product>();
        for (int i = 0; i < 20; i++)
        {
            var product = new ProductBuilder()
                .WithName($"Product {i + 1}")
                .WithImageUrl($"ImageUrl{i + 1}")
                .WithDescription($"Description for product {i + 1}")
                .WithPrice(10 + i)
                .WithStock(5 + i)
                .Build();

            products.Add(product);
        }
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: "EshopDB")
            .Options;
        await using (var context = new ProductDbContext(options))
        {
            context.Products.AddRange(products);
            await context.SaveChangesAsync(CancellationToken.None);
        }
        await using (var context = new ProductDbContext(options))
        {
            var productRepo = new ProductRepository(context);
            var categoryRepoMock = new Mock<ICategoryRepository>();
            var service = new ProductDomainService(productRepo, categoryRepoMock.Object, mockPublishEndpoint.Object);
            var controller = new ProductV2Controller(service);

            // Act
            var result = await controller.GetProducts(false, 2, 5) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var returnedProducts = Assert.IsType<List<ProductDto>>(result.Value);
            Assert.Equal(5, returnedProducts.Count);
            Assert.Equal("Product 6", returnedProducts[0].Name); // First product on page 2
        }
    }
}
