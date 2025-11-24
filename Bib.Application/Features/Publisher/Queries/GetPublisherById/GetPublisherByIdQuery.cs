using Bib.Application.Common.Models;
using Bib.Application.Features.Publisher.Queries.Common;
using MediatR;

namespace Bib.Application.Features.Publisher.Queries.GetPublisherById
{
    public record GetPublisherByIdQuery : IRequest<Result<PublisherDto>>
    {
        public int Id { get; set; }
    };
}
