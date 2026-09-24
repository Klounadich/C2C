using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Payments.Infrastructure.Persistence;

namespace Payments;

public static class PaymentsModuleExtensions
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            
            
        });

        
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("C2CDBConnection"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();
        
        services.AddDbContext<PaymentDBContext>(options =>
            options.UseNpgsql(dataSource, b => b.MigrationsAssembly(typeof(PaymentDBContext).Assembly.FullName)));

        return services;
    }
}