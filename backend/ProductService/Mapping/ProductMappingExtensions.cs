using System;
using Core.Builders;
using Core.Contracts;
using Core.Domain;
using ProductService.DTOs;

namespace ProductService.Mapping;

public static class ProductMappingExtensions
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            ImageUrl = product.ImageUrl,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,
            CategoryNames = product.Categories.Select(c => c.Name).ToList()
        };
    }

    public static Product ToEntity(this CreateProductDto dto, List<Category> categories)
    {
        return new ProductBuilder()
            .WithName(dto.Name)
            .WithImageUrl(dto.ImageUrl)
            .WithDescription(dto.Description)
            .WithPrice(dto.Price)
            .WithStock(dto.StockQuantity)
            .WithCategories(categories)
            .Build();
    }

    public static void ApplyUpdate(this Product product, UpdateProductDto dto, List<Category>? categories = null)
    {
        // Name cannot be changed
        if (dto.ImageUrl is not null) product.ImageUrl = dto.ImageUrl;
        if (dto.Description is not null) product.Description = dto.Description;
        if (dto.Price.HasValue) product.Price = dto.Price.Value;
        if (dto.StockQuantity.HasValue) product.StockQuantity = dto.StockQuantity.Value;
        if (categories is not null) product.Categories = categories;

        product.UpdatedAt = DateTime.UtcNow;
    }

    public static void UpdateStock(this Product product, int newStockQuantity)
    {
        product.StockQuantity = newStockQuantity;
        product.UpdatedAt = DateTime.UtcNow;
    }

    public static ProductCreated ToProductCreatedMessage(this Product product)
    {
        return new ProductCreated
        {
            Id = product.Id,
            Name = product.Name,
            ImageUrl = product.ImageUrl,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.CreatedAt,
            CategoryNames = product.Categories.Select(c => c.Name).ToList()
        };
    }

    public static ProductUpdated ToProductUpdatedMessage(this Product product)
    {
        return new ProductUpdated
        {
            Id = product.Id,
            Name = product.Name,
            ImageUrl = product.ImageUrl,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,
            CategoryNames = product.Categories.Select(c => c.Name).ToList()
        };
    }
}
