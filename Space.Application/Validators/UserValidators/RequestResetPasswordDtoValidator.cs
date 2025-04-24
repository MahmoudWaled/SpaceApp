using FluentValidation;
using Space.Application.DTOs.Users;

namespace Space.Application.Validators.UserValidators
{
    public class RequestResetPasswordDtoValidator : AbstractValidator<RequestResetPasswordDto>
    {
        public RequestResetPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}