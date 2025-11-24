using FluentValidation;

namespace Bib.Application.Features.Publisher.Command.CreatePublisher
{
    public class CreatePublisherCommandValidator : AbstractValidator<CreatePublisherCommand>
    {
        public CreatePublisherCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Publisher name is required")
                .MaximumLength(50).WithMessage("Publisher name must not exceed 50 characters");

            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("Description must not exceed 200 characters");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(10).WithMessage("PhoneNumber must not exceed 10 characters");

            RuleFor(x => x.Email)
                .MaximumLength(50).WithMessage("Email must not exceed 50 characters");

            RuleFor(x => x.Site)
                .MaximumLength(100).WithMessage("Site must not exceed 100 characters");
        }
    }
}
