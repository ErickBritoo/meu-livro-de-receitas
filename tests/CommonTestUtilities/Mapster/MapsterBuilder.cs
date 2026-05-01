using Mapster;
using MapsterMapper;
using MyRecipeBook.Application;
using MyRecipeBook.Application.Services.Mapster;

namespace CommonTestUtilities.Mapster;

public static class MapsterTestConfig
{
    public static void Register()
    {
        TypeAdapterConfig.GlobalSettings.Scan(
            typeof(DependencyInjectionExtension).Assembly
        );
    }
}