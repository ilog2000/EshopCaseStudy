using System;
using Core.Builders;
using Core.Domain;
using ProductService.DTOs;

namespace ProductService.Mapping;

public static class CategoryMappingExtensions
{
    public static CategoryDto ToDto(this Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public static Category ToEntity(this CreateCategoryDto dto)
    {
        return new CategoryBuilder()
            .WithName(dto.Name)
            .Build();
    }

    public static void ApplyUpdate(this Category category, UpdateCategoryDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Name))
        {
            category.Name = dto.Name;
        }
    }
}

