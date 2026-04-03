using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Application.MapsterConfig;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RequestRegisterUserJson, User>().Ignore(u => u.Password);
        
    }
}