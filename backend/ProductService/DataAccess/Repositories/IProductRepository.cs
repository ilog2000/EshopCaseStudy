using System;
using Core.Domain;

namespace ProductService.DataAccess.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetPagedAsync(int pageNr, int pageSize);
    Task<IEnumerable<Product>> GetInStockAsync();
    Task<IEnumerable<Product>> GetInStockPagedAsync(int pageNr, int pageSize);
    Task<Product?> GetByIdAsync(Guid id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Guid id);
}
