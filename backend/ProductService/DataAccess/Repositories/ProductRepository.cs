using System;
using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace ProductService.DataAccess.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _dbContext;

    public ProductRepository(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _dbContext.Products
            .Include(p => p.Categories)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetPagedAsync(int pageNr, int pageSize)
    {
        return await _dbContext.Products
            .Include(p => p.Categories)
            .Skip((pageNr - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetInStockAsync()
    {
        return await _dbContext.Products
            .Include(p => p.Categories)
            .Where(p => p.StockQuantity > 0)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetInStockPagedAsync(int pageNr, int pageSize)
    {
        return await _dbContext.Products
            .Include(p => p.Categories)
            .Where(p => p.StockQuantity > 0)
            .Skip((pageNr - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Products
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> AddAsync(Product product)
    {
        _dbContext.Products.Add(product);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        _dbContext.Entry(product).State = EntityState.Modified;
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product != null)
        {
            _dbContext.Products.Remove(product);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
        return false;
    }
}
