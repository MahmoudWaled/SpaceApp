using FluentValidation;
using Space.Application.DTOs.Posts;

namespace Space.Application.Validators.Post_validator
{
    public class UpdatePostDtoValidator : AbstractValidator<UpdatePostDto>
    {
        public UpdatePostDtoValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("PostId is required.");

            RuleFor(x => x)
                .Must(dto => !string.IsNullOrWhiteSpace(dto.TextContent) || dto.ImagePath != null)
                .WithMessage("You must provide either text content or an image.");
        }
    }
}