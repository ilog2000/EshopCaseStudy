using System;
using Core.Contracts;
using MassTransit;
using ProductService.DataAccess.Repositories;
using ProductService.DTOs;
using ProductService.Mapping;

namespace ProductService.Services;

public class ProductDomainService(IProductRepository productRepo, ICategoryRepository categoryRepo, IPublishEndpoint publishEndpoint) : IProductDomainService
{
    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var categories = await categoryRepo.GetByIdsAsync(dto.CategoryIds);
        var product = dto.ToEntity(categories.ToList());

        await publishEndpoint.Publish(product.ToProductCreatedMessage());

        await productRepo.AddAsync(product);
        return product.ToDto();
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var product = await productRepo.GetByIdAsync(id);
        return product?.ToDto();
    }

    public async Task<List<ProductDto>> GetAllAsync(bool inStock = false)
    {
        if (inStock)
        {
            var inStockProducts = await productRepo.GetInStockAsync();
            return inStockProducts.Select(p => p.ToDto()).ToList();
        }

        // Default to get all products
        var products = await productRepo.GetAllAsync();
        return products.Select(p => p.ToDto()).ToList();
    }

    public async Task<List<ProductDto>> GetPagedAsync(bool inStock = false, int pageNr = 1, int pageSize = 10)
    {
        if (inStock)
        {
            var inStockProducts = await productRepo.GetInStockPagedAsync(pageNr, pageSize);
            return inStockProducts.Select(p => p.ToDto()).ToList();
        }

        var products = await productRepo.GetPagedAsync(pageNr, pageSize);
        return products.Select(p => p.ToDto()).ToList();
    }

    public async Task<ProductDto?> UpdateAsync(Guid id, UpdateProductDto dto)
    {
        var product = await productRepo.GetByIdAsync(id);
        if (product is null) return null;
        // No updates allowed if out of stock
        if (product.StockQuantity == 0) return product.ToDto();

        var categories = dto.CategoryIds is not null
            ? await categoryRepo.GetByIdsAsync(dto.CategoryIds)
            : null;

        product.ApplyUpdate(dto, categories?.ToList());

        await publishEndpoint.Publish(product.ToProductUpdatedMessage());

        await productRepo.UpdateAsync(product);
        return product.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await productRepo.GetByIdAsync(id);
        if (product is null) return false;

        await publishEndpoint.Publish(new ProductDeleted { Id = id });

        await productRepo.DeleteAsync(id);
        return true;
    }

    public async Task<ProductDto?> UpdateProductStockAsync(Guid id, int newStockQuantity)
    {
        var product = await productRepo.GetByIdAsync(id);
        if (product is null) return null;

        product.StockQuantity = newStockQuantity;

        await publishEndpoint.Publish(product.ToProductUpdatedMessage());

        await productRepo.UpdateAsync(product);
        return product.ToDto();
    }
}
