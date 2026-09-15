using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Profile.Infrastructure;
using Profile.Repositories;

namespace Profile;


public static class ProfileModuleExtensions
{
    public static IServiceCollection AddProfileModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ProfileModuleExtensions).Assembly));
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("C2CDBConnection"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();
        
        services.AddDbContext<ProfileDBContext>(options =>
            options.UseNpgsql(dataSource, b => b.MigrationsAssembly(typeof(ProfileDBContext).Assembly.FullName)));

        return services;
    }
}