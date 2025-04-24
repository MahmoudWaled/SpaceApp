using FluentValidation;
using Space.Application.DTOs.Users;

namespace Space.Application.Validators.UserValidators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number.");

            RuleFor(x => x.Bio)
                .MaximumLength(500).WithMessage("Bio cannot exceed 500 characters.")
                .When(x => x.Bio != null);

            RuleFor(x => x.profileImagePath)
              .Must(file => file == null || file.Length <= 5 * 1024 * 1024).WithMessage("Profile image size must not exceed 5 MB.")
              .Must(file => file == null || new[] { ".jpg", ".jpeg", ".png" }.Contains(Path.GetExtension(file.FileName).ToLower())).WithMessage("Profile image must be a JPG, JPEG, or PNG file.");
        }
    }
}