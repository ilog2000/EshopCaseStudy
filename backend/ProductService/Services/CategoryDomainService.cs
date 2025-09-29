using System;
using ProductService.DataAccess.Repositories;
using ProductService.DTOs;
using ProductService.Mapping;

namespace ProductService.Services;

public class CategoryDomainService : ICategoryDomainService
{
    private readonly ICategoryRepository _categoryRepo;

    public CategoryDomainService(ICategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        var category = dto.ToEntity();
        await _categoryRepo.AddAsync(category);
        return category.ToDto();
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        return category?.ToDto();
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var categories = await _categoryRepo.GetAllAsync();
        return categories.Select(c => c.ToDto()).ToList();
    }

    public async Task<CategoryDto?> UpdateAsync(Guid id, UpdateCategoryDto dto)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category is null) return null;

        category.ApplyUpdate(dto);
        await _categoryRepo.UpdateAsync(category);
        return category.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category is null) return false;

        await _categoryRepo.DeleteAsync(id);
        return true;
    }
}

