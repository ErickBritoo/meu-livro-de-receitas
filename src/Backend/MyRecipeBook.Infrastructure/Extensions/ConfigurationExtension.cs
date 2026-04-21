using Microsoft.Extensions.Configuration;
using MyRecipeBook.Infrastructure.Extensions;

namespace MyRecipeBook.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static string ConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("DefaultConnection")!;
    }
}