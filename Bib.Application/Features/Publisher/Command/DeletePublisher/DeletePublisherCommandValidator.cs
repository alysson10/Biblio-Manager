using FluentValidation;

namespace Bib.Application.Features.Publisher.Command.DeletePublisher
{
    public class DeletePublisherCommandValidator : AbstractValidator<DeletePublisherCommand>
    {
        public DeletePublisherCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Publisher Id is required");
        }
    }
}
