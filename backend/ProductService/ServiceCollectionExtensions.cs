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
        services.AddScoped<IProductDomainServce, ProductDomainServce>();
        services.AddScoped<ICategoryDomainServce, CategoryDomainServce>();

        return services;
    }
}
