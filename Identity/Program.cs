using Identity.Handlers;
using Identity.Infrastructure;
using Identity.Repositories;
using Identity.Services;
using Microsoft.AspNetCore.Builder; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Identity;

public static class IdentityModuleExtensions
{
    
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJWTService, JWTService>();
        services.Configure<JWTService.AuthSettings>(
            configuration.GetSection("AuthSettings")
        );
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(RegisterHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(AuthHandler).Assembly);
            
        });

        
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("C2CDBConnection"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();
        
        services.AddDbContext<IdentityDBContext>(options =>
            options.UseNpgsql(dataSource, b => b.MigrationsAssembly(typeof(IdentityDBContext).Assembly.FullName)));

        return services;
    }

    
    public static IApplicationBuilder UseIdentityModule(this IApplicationBuilder app)
    {
        return app;
    }
}