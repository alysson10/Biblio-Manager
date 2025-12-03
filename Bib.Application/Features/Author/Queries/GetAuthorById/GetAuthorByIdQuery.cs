using Bib.Application.Common.Models;
using Bib.Application.Features.Publisher.Queries.Common;
using MediatR;

namespace Bib.Application.Features.Author.Queries.GetAuthorById
{
    public record GetAuthorByIdQuery : IRequest<Result<AuthorDto>>
    {
        public int Id { get; set; }
    }
}
