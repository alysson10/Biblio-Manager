using AutoMapper;
using Bib.Application.Common.Interfaces;
using Bib.Application.Common.Models;
using Bib.Application.Features.Publisher.Queries.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bib.Application.Features.Author.Queries.GetAuthorById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, Result<AuthorDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAuthorByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AuthorDto>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            var publishersEntity = await _unitOfWork.AuthorRepository.GetByIdAsync(request.Id);
            if (publishersEntity is null)
                return Result<AuthorDto>.Failure(Error.NotFound);

            var authorDto = _mapper.Map<AuthorDto>(publishersEntity);

            return Result<AuthorDto>.Success(authorDto);
        }
    }
}
