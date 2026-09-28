using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Payments.Application.Abstractions;
using Payments.Application.Commands.CreatePayment;
using Payments.Infrastructure.Persistence;
using Payments.Infrastructure.Repositories;

namespace Payments;

public static class PaymentsModuleExtensions
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreatePaymentHandler).Assembly);
            
        });

        
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("C2CDBConnection"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();
        
        services.AddDbContext<PaymentDBContext>(options =>
            options.UseNpgsql(dataSource, b => b.MigrationsAssembly(typeof(PaymentDBContext).Assembly.FullName)));

        return services;
    }
}