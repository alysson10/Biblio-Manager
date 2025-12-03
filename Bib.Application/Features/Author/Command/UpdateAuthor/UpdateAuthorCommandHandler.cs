using AutoMapper;
using Bib.Application.Common.Behaviors;
using Bib.Application.Common.Interfaces;
using Bib.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bib.Application.Features.Author.Command.UpdateAuthor
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public UpdateAuthorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new UpdateAuthorCommandValidator();
                var valid = validator.Validate(command);
                if (!valid.IsValid)
                {
                    return Result<int>.Failure(
                        new Error(valid.Errors.FirstOrDefault().ErrorCode,
                                  valid.Errors.FirstOrDefault().ErrorMessage));
                }

                var authorEntity = await _unitOfWork.AuthorRepository.GetByIdAsync(command.Id);
                if (authorEntity is null)
                    return Result<int>.Failure(Error.NotFound);

                var userId = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value!;
                var newPublishersEntity = new Domain.Entities.Author(int.Parse(userId), command.Name, command.Email, command.PhoneNumber, command.Status, DateTime.Now);

                if (!ComparisonHelper.Changed(authorEntity, newPublishersEntity))
                    return Result<int>.Failure(Error.NotChange);

                authorEntity.UpdatePublisher(command.Name, command.Email, command.PhoneNumber, command.Status);

                var publisherId = await _unitOfWork.AuthorRepository.UpdateAsync(authorEntity);
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
