using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCase.User.Register;
using MyRecipeBook.Exceptions;
using Shouldly;
using Xunit;

namespace Validators.Test.User.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new RegisterUserValidation();

        var request = RequestRegisterUserJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var validator = new RegisterUserValidation();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        var error = result.Errors.ShouldHaveSingleItem();
        error.ErrorMessage.ShouldBe(ResourceMessagesException.NAME_EMPTY);
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var validator = new RegisterUserValidation();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        var error = result.Errors.ShouldHaveSingleItem();
        error.ErrorMessage.ShouldBe(ResourceMessagesException.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
         var validator = new RegisterUserValidation();
 
         var request = RequestRegisterUserJsonBuilder.Build();
         request.Email = "email.com";
 
         var result = validator.Validate(request);
 
         result.IsValid.ShouldBeFalse();
         var error = result.Errors.ShouldHaveSingleItem();
         error.ErrorMessage.ShouldBe(ResourceMessagesException.EMAIL_INVALID);
     }
 
     [Theory]
     [InlineData(0)]
     [InlineData(1)]
     [InlineData(2)]
     [InlineData(3)]
     [InlineData(4)]
     [InlineData(5)]
     public void Error_Password_Invalid(int passwordLength)
     {
         var validator = new RegisterUserValidation();
 
         var request = RequestRegisterUserJsonBuilder.Build(passwordLength);
 
         var result = validator.Validate(request);
 
         result.IsValid.ShouldBeFalse();
         var error = result.Errors.ShouldHaveSingleItem();
         error.ErrorMessage.ShouldBe(ResourceMessagesException.PASSWORD_SIZE_CHARACTERES);
     }
 }