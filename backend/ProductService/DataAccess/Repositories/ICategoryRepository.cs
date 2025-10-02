using System;
using Core.Domain;

namespace ProductService.DataAccess.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid id);
    Task<IEnumerable<Category>> GetByIdsAsync(IEnumerable<Guid> ids);
    Task<bool> AddAsync(Category category);
    Task<bool> UpdateAsync(Category category);
    Task<bool> DeleteAsync(Guid id);
}
