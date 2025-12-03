using Bib.Application.Common.Models;
using Bib.Application.Features.Publisher.Queries.Common;
using MediatR;
using System.Collections.Generic;

namespace Bib.Application.Features.Author.Queries.GetAuthors
{
    public record GetAuthorsQuery : IRequest<Result<IEnumerable<AuthorDto>>>;
}
