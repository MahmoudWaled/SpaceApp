using FluentValidation;
using Space.Application.DTOs.Users;

namespace Space.Application.Validators.FollowValidators
{
    public class FollowUserRequestDtoValidator : AbstractValidator<FollowRequestDto>
    {
        public FollowUserRequestDtoValidator()
        {
            RuleFor(x => x.FolloweeId)
                .NotEmpty().WithMessage("Followee ID is required.");
        }
    }
}