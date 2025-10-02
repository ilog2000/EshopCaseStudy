using System;
using Core.Domain;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ProductService.DataAccess;

public class ProductDbContext(DbContextOptions<ProductDbContext> options)
    : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    // Use fluent API to configure the model
    // This way keeps domain entities clean and independent of EF Core
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());

        // MassTransit Outbox and Inbox tables
        // Don't forget to add migrations after adding these
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}
