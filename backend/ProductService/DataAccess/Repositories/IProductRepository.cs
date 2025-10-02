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
    Task<bool> AddAsync(Product product);
    Task<bool> UpdateAsync(Product product);
    Task<bool> DeleteAsync(Guid id);
}
