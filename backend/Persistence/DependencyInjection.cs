using Application.Organizations.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

using Application.Shared.Data;
using Application.Users.Interfaces;
using Persistence.Repositories;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabases(configuration).AddRepositories();
        return services;
    }

    private static IServiceCollection AddDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        var postgresConnectionString = configuration.GetConnectionString("Postgres") 
             ?? throw new NullReferenceException("Postgres connection string is null");
        
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(postgresConnectionString)
                .UseSnakeCaseNamingConvention();
        });
        
        var redisConnectionString = configuration.GetConnectionString("Redis")
            ?? throw new NullReferenceException("Redis connection string is null");
                
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(redisConnectionString));
        
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();
        services.AddScoped<IOrganizationsRepository, OrganizationsRepository>();

        services.AddScoped<IConfirmationCodesRepository, ConfirmationCodesRepository>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
}