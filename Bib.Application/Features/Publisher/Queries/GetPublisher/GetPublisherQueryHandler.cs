using AutoMapper;
using Bib.Application.Common.Interfaces;
using Bib.Application.Common.Models;
using Bib.Application.Features.Publisher.Queries.Common;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bib.Application.Features.Publisher.Queries.GetPublisher
{
    public class GetPublishersQueryHandler : IRequestHandler<GetPublishersQuery, Result<IEnumerable<PublisherDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPublishersQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<PublisherDto>>> Handle(GetPublishersQuery request, CancellationToken cancellationToken)
        {
            var publishersEntity = await _unitOfWork.PublisherRepository.GetAllAsync();

            var productsDto = _mapper.Map<List<PublisherDto>>(publishersEntity);

            return Result<IEnumerable<PublisherDto>>.Success(productsDto);
        }        
    }
}
