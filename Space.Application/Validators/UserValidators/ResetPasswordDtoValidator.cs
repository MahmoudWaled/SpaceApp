using FluentValidation;
using Space.Application.DTOs.Users;

namespace Space.Application.Validators.UserValidators
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required.");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("New password must be at least 6 characters long.")
                .Matches(@"[A-Z]").WithMessage("New password must contain at least one uppercase letter.")
                .Matches(@"[0-9]").WithMessage("New password must contain at least one number.");
        }
    }
}