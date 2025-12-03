using Bib.Application.Common.Interfaces;
using Bib.Application.Common.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bib.Application.Features.Author.Command.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAuthorCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteAuthorCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new DeleteAuthorCommandValidator();
                var valid = validator.Validate(command);
                if (!valid.IsValid)
                {
                    return Result<int>.Failure(
                        new Error(valid.Errors.FirstOrDefault().ErrorCode,
                                  valid.Errors.FirstOrDefault().ErrorMessage));
                }

                var publishersEntity = await _unitOfWork.AuthorRepository.GetByIdAsync(command.Id);
                if (publishersEntity is null)
                    return Result<int>.Failure(Error.NotFound);


                var whereCondition = $"AuthorId = {publishersEntity.Id}";
                var result = await _unitOfWork.BookRepository.GetFirstByConditionAsync(whereCondition);
                if (result)
                    await _unitOfWork.AuthorRepository.DeleteSoftAsync(command.Id);
                else
                    await _unitOfWork.AuthorRepository.DeleteHardAsync(command.Id);

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
