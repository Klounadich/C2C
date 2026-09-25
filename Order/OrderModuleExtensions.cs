using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Order.Infrastructure.Persistance;

namespace Order;

public static class OrderModuleExtensions
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            
            
        });

        
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("C2CDBConnection"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();
        
        services.AddDbContext<OrderDBContext>(options =>
            options.UseNpgsql(dataSource, b => b.MigrationsAssembly(typeof(OrderDBContext).Assembly.FullName)));

        return services;
    }
}