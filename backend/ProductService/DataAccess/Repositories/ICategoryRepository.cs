using System;
using Core.Domain;

namespace ProductService.DataAccess.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid id);
    Task<IEnumerable<Category>> GetByIdsAsync(IEnumerable<Guid> ids);
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Guid id);
}
