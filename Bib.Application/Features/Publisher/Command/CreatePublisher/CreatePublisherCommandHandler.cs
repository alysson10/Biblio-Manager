using Bib.Application.Common.Interfaces;
using Bib.Application.Common.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bib.Application.Features.Publisher.Command.CreatePublisher
{
    public class CreatePublisherCommandHandler : IRequestHandler<CreatePublisherCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePublisherCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreatePublisherCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreatePublisherCommandValidator();
                var valid = validator.Validate(command);
                if (!valid.IsValid)
                {
                    return Result<int>.Failure(
                        new Error(valid.Errors.FirstOrDefault().ErrorCode,
                                  valid.Errors.FirstOrDefault().ErrorMessage));
                }

                var publisher = new Domain.Entities.Publisher(command.Name, command.Description, command.PhoneNumber, command.Email, command.Site, true);

                var publisherId = await _unitOfWork.PublisherRepository.CreateAsync(publisher);
                await _unitOfWork.CommitAsync();

                return Result<int>.Success(publisherId);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                return Result<int>.Failure(Error.Database);
            }
        }
    }
}
