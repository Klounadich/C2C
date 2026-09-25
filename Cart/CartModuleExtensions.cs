
using Cart.Handlers;
using Cart.Infrastructure;
using Cart.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

public static class CartModuleExtensions
{
    public static IServiceCollection AddCartModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICartRepository, CartRepository>();
        

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(AddItemHandler).Assembly);
          
            
        });
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("C2CDBConnection"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();
        
        services.AddDbContext<CartDBContext>(options =>
            options.UseNpgsql(dataSource, b => b.MigrationsAssembly(typeof(CartDBContext).Assembly.FullName)));

        return services;
    }
}