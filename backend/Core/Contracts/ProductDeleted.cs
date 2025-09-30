using System;

namespace Core.Contracts;

public class ProductDeleted
{
    public required Guid Id { get; set; }
}
