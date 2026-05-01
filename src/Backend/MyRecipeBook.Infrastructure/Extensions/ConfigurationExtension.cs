using Microsoft.Extensions.Configuration;
using MyRecipeBook.Infrastructure.Extensions;
using static System.Boolean;

namespace MyRecipeBook.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static bool IsUnitTestEnviroment(this IConfiguration configuration)
    {
        var value = configuration["InMemoryTest"];

        if (!bool.TryParse(value, out var result))
            throw new InvalidOperationException("Configuração de teste 'InMemoryTest' inválida ");
        
        return result;
    }
    public static string ConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("DefaultConnection")!;
    }
}