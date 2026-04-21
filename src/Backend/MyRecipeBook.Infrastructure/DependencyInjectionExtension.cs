using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Infrastructure.DataAcess;
using MyRecipeBook.Infrastructure.DataAcess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddDbContext(services, configuration);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        
        var serverVersion = new MySqlServerVersion(ServerVersion.AutoDetect(connectionString));
        
        services.AddDbContext<MyRecipeBookDbContext>(options =>
        {
            options.UseMySql(connectionString, serverVersion);
        });
    }
}