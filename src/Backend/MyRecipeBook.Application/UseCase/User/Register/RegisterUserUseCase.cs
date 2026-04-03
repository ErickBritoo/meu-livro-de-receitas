using System.Globalization;
using FluentValidation.Results;
using Mapster;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCase.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly PasswordEncripter _passwordEncripter;
    private readonly IUnitOfWork _unitOfWork;
    
    public RegisterUserUseCase(
        IUserReadOnlyRepository readOnlyRepository, 
        IUserWriteOnlyRepository writeOnlyRepository,
        PasswordEncripter passwordEncripter,
        IUnitOfWork unitOfWork
        )
    {
        _readOnlyRepository = readOnlyRepository;
        _writeOnlyRepository = writeOnlyRepository;
        _passwordEncripter = passwordEncripter;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
    {
        // Todos users cases devem fazer os seguintes passos: validar requisição, mapear entidade, criptografar senha e salvar
        
        await Validate(request);

        var user = request.Adapt<Domain.Entities.User>();

        user.Password = _passwordEncripter.Encrypt(request.Password);

        await _writeOnlyRepository.Add(user);

        await _unitOfWork.Commit();
        
        return new ResponseRegisterUserJson
        {
            Name = request.Name
        };  
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidation();
        var result = validator.Validate(request);

        if (await _readOnlyRepository.ExistActiveUserWithEmail(request.Email))
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
        
        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}