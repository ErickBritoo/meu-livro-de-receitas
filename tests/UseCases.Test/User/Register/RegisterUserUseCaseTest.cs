using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapster;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCase.User.Register;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using Xunit;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase();
        
        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
    }
    
    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);
        
        var exception = await Should.ThrowAsync<ErrorOnValidationException>(
            async () => await useCase.Execute(request)
        );        
        
        exception.ErrorMessages.Count.ShouldBe(1);
        exception.ErrorMessages.Single().ShouldBe(ResourceMessagesException.EMAIL_ALREADY_REGISTERED);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();
        
        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(request));
        
        exception.ErrorMessages.Count.ShouldBe(1);
        exception.ErrorMessages.Single().ShouldBe(ResourceMessagesException.NAME_EMPTY);
    }
    
    private static RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var readOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        
        if (email is not null)
            readOnlyRepositoryBuilder.ExistActiveUserWithEmail(email);
        
        var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var readOnlyRepository = readOnlyRepositoryBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        MapsterTestConfig.Register();


        return new RegisterUserUseCase(readOnlyRepository, writeOnlyRepository, passwordEncripter, unitOfWork);
    }
}