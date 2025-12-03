using FluentValidation;

namespace Bib.Application.Features.Author.Command.DeleteAuthor
{
    public class DeleteAuthorCommandValidator : AbstractValidator<DeleteAuthorCommand>
    {
        public DeleteAuthorCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Author Id is required");
        }
    }
}
