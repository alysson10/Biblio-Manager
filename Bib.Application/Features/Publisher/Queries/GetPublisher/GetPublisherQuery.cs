using Bib.Application.Common.Models;
using Bib.Application.Features.Publisher.Queries.Common;
using MediatR;
using System.Collections.Generic;

namespace Bib.Application.Features.Publisher.Queries.GetPublisher
{
    public record GetPublishersQuery : IRequest<Result<IEnumerable<PublisherDto>>>;
}
