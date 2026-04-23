using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCase.User.Register;

public class RegisterUserValidation : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidation()
    {
        RuleFor(u => u.Name).NotEmpty().WithMessage( _ => ResourceMessagesException.NAME_EMPTY);
        RuleFor(u => u.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
        RuleFor(u => u.Password).MinimumLength(6).WithMessage(ResourceMessagesException.PASSWORD_SIZE_CHARACTERES);
        
        When(user => !string.IsNullOrEmpty(user.Email), () =>
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
        });
    }
}