using System;
using ProductService.DTOs;

namespace ProductService.Services;

public interface IProductDomainService
{
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<ProductDto?> GetByIdAsync(Guid id);
    Task<List<ProductDto>> GetAllAsync(bool inStock = false);
    Task<List<ProductDto>> GetPagedAsync(bool inStock = false, int pageNr = 1, int pageSize = 10);
    Task<ProductDto?> UpdateAsync(Guid id, UpdateProductDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<ProductDto?> UpdateProductStockAsync(Guid id, int newStockQuantity);
}
