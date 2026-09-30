using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Payments.Application.Abstractions;
using Payments.Application.Commands.CreatePayment;
using Payments.Infrastructure.Persistence;
using Payments.Infrastructure.Repositories;
using Payments.Services;

namespace Payments;

public static class PaymentsModuleExtensions
{
    public static IServiceCollection AddPaymentModule(this IServiceCollection services, IConfiguration configuration)
    {
        
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter()
                );
            });
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ProviderService>();
        services.AddHttpClient<ProviderService>();
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