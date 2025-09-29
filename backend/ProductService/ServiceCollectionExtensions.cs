using System;
using ProductService.DataAccess.Repositories;
using ProductService.Services;

namespace ProductService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        // Services
        services.AddScoped<IProductDomainService, ProductDomainService>();
        services.AddScoped<ICategoryDomainService, CategoryDomainService>();

        return services;
    }
}
