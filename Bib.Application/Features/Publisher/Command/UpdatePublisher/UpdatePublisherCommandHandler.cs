using AutoMapper;
using Bib.Application.Common.Behaviors;
using Bib.Application.Common.Interfaces;
using Bib.Application.Common.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bib.Application.Features.Publisher.Command.UpdatePublisher
{
    public class UpdatePublisherCommandHandler : IRequestHandler<UpdatePublisherCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public UpdatePublisherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(UpdatePublisherCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new UpdatePublisherCommandValidator();
                var valid = validator.Validate(command);
                if (!valid.IsValid)
                {
                    return Result<int>.Failure(
                        new Error(valid.Errors.FirstOrDefault().ErrorCode,
                                  valid.Errors.FirstOrDefault().ErrorMessage));
                }

                var publishersEntity = await _unitOfWork.PublisherRepository.GetByIdAsync(command.Id);
                if (publishersEntity is null)
                    return Result<int>.Failure(Error.NotFound);

                var newPublishersEntity = new Domain.Entities.Publisher(command.Name, command.Description, command.PhoneNumber, command.Email, command.Site, command.Status);
                
                if (!ComparisonHelper.Changed(publishersEntity, newPublishersEntity))
                    return Result<int>.Failure(Error.NotChange);

                publishersEntity.UpdatePublisher(command.Name, command.Description, command.PhoneNumber, command.Email, command.Site, command.Status);

                var publisherId = await _unitOfWork.PublisherRepository.UpdateAsync(publishersEntity);
                await _unitOfWork.CommitAsync();

                return Result<int>.Success(command.Id);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                return Result<int>.Failure(Error.Database);
            }
        }
    }
}
