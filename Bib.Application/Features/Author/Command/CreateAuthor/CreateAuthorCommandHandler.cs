using Bib.Application.Common.Interfaces;
using Bib.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bib.Application.Features.Author.Command.CreateAuthor
{
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public CreateAuthorCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result<int>> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateAuthorCommandValidator();
                var valid = validator.Validate(command);
                if (!valid.IsValid)
                {
                    return Result<int>.Failure(
                        new Error(valid.Errors.FirstOrDefault().ErrorCode,
                                  valid.Errors.FirstOrDefault().ErrorMessage));
                }

                var userId = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value!;
                var author = new Domain.Entities.Author(int.Parse(userId), command.Name, command.Email, command.PhoneNumber, true);

                var authorId = await _unitOfWork.AuthorRepository.CreateAsync(author);
                await _unitOfWork.CommitAsync();

                return Result<int>.Success(authorId);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                return Result<int>.Failure(Error.Database);
            }
        }
    }
}
