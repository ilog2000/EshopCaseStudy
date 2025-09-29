using System;
using ProductService.DTOs;

namespace ProductService.Services;

public interface ICategoryDomainService
{
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
    Task<CategoryDto?> GetByIdAsync(Guid id);
    Task<List<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> UpdateAsync(Guid id, UpdateCategoryDto dto);
    Task<bool> DeleteAsync(Guid id);
}
