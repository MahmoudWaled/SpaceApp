using FluentValidation;
using Space.Application.DTOs.Comments;

namespace Space.Application.Validators.CommentValidators
{
    public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Comment content is required.")
                .MaximumLength(1000).WithMessage("Comment cannot exceed 1000 character");

            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("PostId is required.");
        }
    }
}