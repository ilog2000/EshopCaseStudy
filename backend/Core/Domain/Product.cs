using System;

namespace Core.Domain;

public class Product
{
    public Product(string name, string? imageUrl = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = string.Empty;
        ImageUrl = imageUrl;
        Price = 0;
        StockQuantity = 0;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } // Name is immutable after creation
    public required string Description { get; set; }
    public string? ImageUrl { get; set; } // ImageUrl is optional, should be replaced with default image if null
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<Category> Categories { get; set; } = new();
}
