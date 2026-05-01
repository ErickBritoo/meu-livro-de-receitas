using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCase.Login.DoLogin;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using Xunit;

namespace UseCases.Test.Login.DoLogin;

public class DoLoginUseCaseTest 
{
    [Fact]
    public async Task Sucess()
    {
        var (user, password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(new RequestLoginJson
        {
            Email = user.Email,
            Password = password
        });
        
        result.ShouldNotBeNull();
        result.Name.ShouldNotBeNullOrWhiteSpace();
        result.Name.ShouldBe(user.Name);
    }
    
    [Fact]
    public async Task Error_Invalid_User()
    {
        var request = RequestLoginJsonBuilder.Build();
            
        var useCase = CreateUseCase();

        Func<Task> act = async () => { await useCase.Execute(request); };

        var exception = await act.ShouldThrowAsync<InvalidLoginException>();

        exception.Message.ShouldBe(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID);
    }
    
    private static DoLoginUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null)
    {
        var readOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        
        if (user is not null)
            readOnlyRepositoryBuilder.GetByEmailAndPassword(user);
        
        return new DoLoginUseCase(readOnlyRepositoryBuilder.Build(), passwordEncripter);
    }
}