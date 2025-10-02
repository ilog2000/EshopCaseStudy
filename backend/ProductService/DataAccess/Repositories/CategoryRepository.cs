using System;
using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace ProductService.DataAccess.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ProductDbContext _dbContext;

    public CategoryRepository(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _dbContext.Categories.ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Categories.FindAsync(id);
    }

    public async Task<IEnumerable<Category>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        return await _dbContext.Categories.Where(c => ids.Contains(c.Id)).ToListAsync();
    }

    public async Task<bool> AddAsync(Category category)
    {
        _dbContext.Categories.Add(category);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> UpdateAsync(Category category)
    {
        _dbContext.Entry(category).State = EntityState.Modified;
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category != null)
        {
            _dbContext.Categories.Remove(category);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
        return false;
    }
}
