using System;
using Core.Domain;

namespace Core.Builders;

public class ProductBuilder
{
    private string _name = string.Empty;
    private string? _imageUrl;
    private string _description = string.Empty;
    private decimal _price = 0;
    private int _stockQuantity = 0;
    private List<Category> _categories = new();

    public ProductBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductBuilder WithImageUrl(string? imageUrl)
    {
        _imageUrl = imageUrl;
        return this;
    }

    public ProductBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ProductBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    public ProductBuilder WithStock(int quantity)
    {
        _stockQuantity = quantity;
        return this;
    }

    public ProductBuilder WithCategories(List<Category> categories)
    {
        _categories = categories;
        return this;
    }

    public Product Build()
    {
        if (string.IsNullOrWhiteSpace(_name))
            throw new InvalidOperationException("Product name is required.");

        var product = new Product(_name, _imageUrl)
        {
            Description = _description,
            Price = _price,
            StockQuantity = _stockQuantity,
            Categories = _categories
        };
        return product;
    }
}
