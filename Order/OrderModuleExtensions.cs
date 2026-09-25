using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Order.Application.Abstractions;
using Order.Application.Commands.CreateOrder;
using Order.Infrastructure.Persistance;
using Order.Infrastructure.Repositories;

namespace Order;

public static class OrderModuleExtensions
{
    public static IServiceCollection AddOrderModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddMediatR(cfg =>
        {
            
            cfg.RegisterServicesFromAssembly(typeof(CreateOrderHandler).Assembly);
        });

        
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("C2CDBConnection"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();
        
        services.AddDbContext<OrderDBContext>(options =>
            options.UseNpgsql(dataSource, b => b.MigrationsAssembly(typeof(OrderDBContext).Assembly.FullName)));

        return services;
    }
}