using System;
using Core.Domain;

namespace Core.Builders;

public class CategoryBuilder
{
    private string _name = string.Empty;

    public CategoryBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public Category Build()
    {
        if (string.IsNullOrWhiteSpace(_name))
            throw new InvalidOperationException("Category name cannot be empty.");

        return new Category(_name);
    }
}
