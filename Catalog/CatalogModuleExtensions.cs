
using Catalog.Infrastructure;
using Catalog.Repositories;
using Catalog.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Catalog;

public static class CatalogModuleExtensions
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AliasSearcher>();
        services.AddScoped<SearchNormalizer>();
        services.AddScoped<SynonymousWordsParser>();
        services.AddScoped<ICatalogRepository, CatalogRepository>();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(FindItemHandler).Assembly);
          
            
        });
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("C2CDBConnection"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();
        
        services.AddDbContext<CatalogDBContext>(options =>
            options.UseNpgsql(dataSource, b => b.MigrationsAssembly(typeof(CatalogDBContext).Assembly.FullName)));

        return services;
    }

    
}