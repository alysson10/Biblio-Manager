using AutoMapper;
using Bib.Application.Common.Interfaces;
using Bib.Application.Common.Models;
using Bib.Application.Features.Publisher.Queries.Common;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bib.Application.Features.Author.Queries.GetAuthors
{
    public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, Result<IEnumerable<AuthorDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAuthorsQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<AuthorDto>>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
        {
            var publishersEntity = await _unitOfWork.AuthorRepository.GetAllAsync();

            var productsDto = _mapper.Map<List<AuthorDto>>(publishersEntity);

            return Result<IEnumerable<AuthorDto>>.Success(productsDto);
        }
    }
}
