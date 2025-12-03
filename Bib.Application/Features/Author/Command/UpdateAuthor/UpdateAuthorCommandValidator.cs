using FluentValidation;

namespace Bib.Application.Features.Author.Command.UpdateAuthor
{
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Publisher name is required")
                .MaximumLength(50).WithMessage("Publisher name must not exceed 50 characters");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(10).WithMessage("PhoneNumber must not exceed 10 characters");

            RuleFor(x => x.Email)
                .MaximumLength(50).WithMessage("Email must not exceed 50 characters");
        }
    }
}
