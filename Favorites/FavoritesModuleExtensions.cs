using Favorites.Handlers;
using Favorites.Infastructure;
using Favorites.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Favorites;

public static class FavoritesModuleExtensions
{
    public static IServiceCollection AddFavoritesModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFavoritesRepository, FavoritesRepository>();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(AddToFavoritesHandler).Assembly);
          
            
        });
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("C2CDBConnection"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();
        
        services.AddDbContext<FavoritesDBContext>(options =>
            options.UseNpgsql(dataSource, b => b.MigrationsAssembly(typeof(FavoritesDBContext).Assembly.FullName)));

        return services;
    }
}