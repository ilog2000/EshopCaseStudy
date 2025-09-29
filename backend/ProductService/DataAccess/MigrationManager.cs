using System;
using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace ProductService.DataAccess;

public static class MigrationManager
{
    public static WebApplication MigrateDatabase(this WebApplication webApp)
    {
        using (var scope = webApp.Services.CreateScope())
        {
            using (var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>())
            {
                try
                {
                    dbContext.Database.Migrate();
                    SeedDatabase(dbContext);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
                    throw;
                }
            }
        }

        return webApp;
    }

    private static void SeedDatabase(ProductDbContext dbContext)
    {
        // Seed initial data if necessary
        if (!dbContext.Categories.Any())
        {
            dbContext.Categories.AddRange(
                new Category("Electronics"),
                new Category("Clothing"),
                new Category("Books")
            );
            dbContext.SaveChanges();
            Console.WriteLine("Seeded initial categories.");
        }

        if (!dbContext.Products.Any())
        {
            var electronicsCategory = dbContext.Categories.First(c => c.Name == "Electronics");
            var clothingCategory = dbContext.Categories.First(c => c.Name == "Clothing");
            var booksCategory = dbContext.Categories.First(c => c.Name == "Books");

            dbContext.Products.AddRange(
                new Product("Smartphone") { Description = "Latest model smartphone", Price = 699.99m, StockQuantity = 50, Categories = new List<Category> { electronicsCategory } },
                new Product("Laptop") { Description = "High performance laptop", Price = 1299.99m, StockQuantity = 30, Categories = new List<Category> { electronicsCategory } },
                new Product("Tablet") { Description = "Portable tablet with stylus", Price = 499.99m, StockQuantity = 20, Categories = new List<Category> { electronicsCategory } },
                new Product("Smartwatch") { Description = "Feature-rich smartwatch", Price = 199.99m, StockQuantity = 80, Categories = new List<Category> { electronicsCategory } },
                new Product("Headphones") { Description = "Noise-cancelling headphones", Price = 299.99m, StockQuantity = 60, Categories = new List<Category> { electronicsCategory } },

                new Product("T-Shirt") { Description = "100% cotton t-shirt", Price = 19.99m, StockQuantity = 100, Categories = new List<Category> { clothingCategory } },
                new Product("Hoodie") { Description = "Warm and cozy hoodie", Price = 39.99m, StockQuantity = 50, Categories = new List<Category> { clothingCategory } },
                new Product("Jeans") { Description = "Comfortable blue jeans", Price = 49.99m, StockQuantity = 75, Categories = new List<Category> { clothingCategory } },
                new Product("Jacket") { Description = "Waterproof jacket", Price = 89.99m, StockQuantity = 40, Categories = new List<Category> { clothingCategory } },
                new Product("Sneakers") { Description = "Stylish sneakers", Price = 59.99m, StockQuantity = 90, Categories = new List<Category> { clothingCategory } },

                new Product("Novel") { Description = "Bestselling fiction novel", Price = 14.99m, StockQuantity = 200, Categories = new List<Category> { booksCategory } },
                new Product("Biography") { Description = "Inspiring life story", Price = 24.99m, StockQuantity = 150, Categories = new List<Category> { booksCategory } },
                new Product("Science Book") { Description = "Comprehensive science textbook", Price = 59.99m, StockQuantity = 80, Categories = new List<Category> { booksCategory } },
                new Product("History Book") { Description = "Detailed history book", Price = 29.99m, StockQuantity = 120, Categories = new List<Category> { booksCategory } },
                new Product("Children's Book") { Description = "Fun and educational children's book", Price = 9.99m, StockQuantity = 250, Categories = new List<Category> { booksCategory } }
            );
            dbContext.SaveChanges();
            Console.WriteLine("Seeded initial products.");
        }
    }
}
