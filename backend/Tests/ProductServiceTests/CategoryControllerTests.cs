using Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductService.Controllers;
using ProductService.DTOs;
using ProductService.Services;

namespace ProductServiceTests;

public class CategoryControllerTests
{
    [Fact]
    public async Task GetCategories_ReturnsOkResult_WithCategories()
    {
        // Arrange
        var mockService = new Mock<ICategoryDomainService>();
        mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<CategoryDto>
        {
            new CategoryDto { Id = Guid.NewGuid(), Name = "Category 1" },
            new CategoryDto { Id = Guid.NewGuid(), Name = "Category 2" }
        });
        var controller = new CategoryController(mockService.Object);

        // Act
        var result = await controller.GetCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var categories = Assert.IsAssignableFrom<List<CategoryDto>>(okResult.Value);
        Assert.Equal(2, categories.Count);
        Assert.Equal("Category 1", categories[0].Name);
        Assert.Equal("Category 2", categories[1].Name);
    }

    [Fact]
    public async Task GetCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var mockService = new Mock<ICategoryDomainService>();
        mockService.Setup(s => s.GetByIdAsync(categoryId)).ReturnsAsync((CategoryDto?)null);
        var controller = new CategoryController(mockService.Object);

        // Act
        var result = await controller.GetCategory(categoryId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateCategory_ReturnsCreatedAtActionResult_WithCreatedCategory()
    {
        // Arrange
        var createDto = new CreateCategoryDto
        {
            Name = "New Category",
        };
        var createdCategory = new CategoryDto
        {
            Id = Guid.NewGuid(),
            Name = createDto.Name,
        };
        var mockService = new Mock<ICategoryDomainService>();
        mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(createdCategory);
        var controller = new CategoryController(mockService.Object);

        // Act
        var result = await controller.CreateCategory(createDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var category = Assert.IsAssignableFrom<CategoryDto>(createdAtActionResult.Value);
        Assert.Equal("New Category", category.Name);
        Assert.Equal(createdCategory.Id, category.Id);
    }

    [Fact]
    public async Task UpdateCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var updateDto = new UpdateCategoryDto
        {
            Name = "Updated Category",
        };
        var mockService = new Mock<ICategoryDomainService>();
        mockService.Setup(s => s.UpdateAsync(categoryId, updateDto)).ReturnsAsync((CategoryDto?)null);
        var controller = new CategoryController(mockService.Object);

        // Act
        var result = await controller.UpdateCategory(categoryId, updateDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteCategory_ReturnsNoContent_WhenDeletionIsSuccessful()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var mockService = new Mock<ICategoryDomainService>();
        mockService.Setup(s => s.DeleteAsync(categoryId)).ReturnsAsync(true);
        var controller = new CategoryController(mockService.Object);

        // Act
        var result = await controller.DeleteCategory(categoryId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
