using System;

namespace ProductService.DTOs;

public class CreateCategoryDto
{
    public required string Name { get; set; }
}

public class UpdateCategoryDto
{
    public string? Name { get; set; }
}

public class CategoryDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}
