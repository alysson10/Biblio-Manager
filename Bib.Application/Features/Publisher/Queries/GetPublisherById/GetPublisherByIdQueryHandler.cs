using AutoMapper;
using Bib.Application.Common.Interfaces;
using Bib.Application.Common.Models;
using Bib.Application.Features.Publisher.Queries.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bib.Application.Features.Publisher.Queries.GetPublisherById
{
    public class GetPublisherByIdQueryHandler : IRequestHandler<GetPublisherByIdQuery, Result<PublisherDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPublisherByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PublisherDto>> Handle(GetPublisherByIdQuery request, CancellationToken cancellationToken)
        {
            var publishersEntity = await _unitOfWork.PublisherRepository.GetByIdAsync(request.Id);
            if (publishersEntity is null)
                return Result<PublisherDto>.Failure(Error.NotFound);

            var productsDto = _mapper.Map<PublisherDto>(publishersEntity);

            return Result<PublisherDto>.Success(productsDto);
        }
    }
}
