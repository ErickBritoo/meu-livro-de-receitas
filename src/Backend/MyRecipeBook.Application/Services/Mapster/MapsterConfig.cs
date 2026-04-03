using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Application.Services.Mapster;

public static class MapsterConfig
{
    public static void Register()
    {
        TypeAdapterConfig<RequestRegisterUserJson, Domain.Entities.User>.NewConfig().Ignore(dest => dest.Password);
    }
}