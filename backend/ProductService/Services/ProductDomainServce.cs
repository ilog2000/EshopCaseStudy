using System;
using ProductService.DataAccess.Repositories;
using ProductService.DTOs;
using ProductService.Mapping;

namespace ProductService.Services;

public class ProductDomainServce : IProductDomainServce
{
    private readonly IProductRepository _productRepo;
    private readonly ICategoryRepository _categoryRepo;

    public ProductDomainServce(IProductRepository productRepo, ICategoryRepository categoryRepo)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var categories = await _categoryRepo.GetByIdsAsync(dto.CategoryIds);
        var product = dto.ToEntity(categories.ToList());
        await _productRepo.AddAsync(product);
        return product.ToDto();
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        return product?.ToDto();
    }

    public async Task<List<ProductDto>> GetAllAsync(bool inStock = false)
    {
        if (inStock)
        {
            var inStockProducts = await _productRepo.GetInStockAsync();
            return inStockProducts.Select(p => p.ToDto()).ToList();
        }

        // Default to get all products
        var products = await _productRepo.GetAllAsync();
        return products.Select(p => p.ToDto()).ToList();
    }

    public async Task<ProductDto?> UpdateAsync(Guid id, UpdateProductDto dto)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product is null) return null;
        // No updates allowed if out of stock
        if (product.StockQuantity == 0) return product.ToDto();

        var categories = dto.CategoryIds is not null
            ? await _categoryRepo.GetByIdsAsync(dto.CategoryIds)
            : null;

        product.ApplyUpdate(dto, categories?.ToList());
        await _productRepo.UpdateAsync(product);
        return product.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product is null) return false;

        await _productRepo.DeleteAsync(id);
        return true;
    }

    public async Task<ProductDto?> UpdateProductStockAsync(Guid id, int newStockQuantity)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product is null) return null;

        product.StockQuantity = newStockQuantity;
        await _productRepo.UpdateAsync(product);
        return product.ToDto();
    }
}
