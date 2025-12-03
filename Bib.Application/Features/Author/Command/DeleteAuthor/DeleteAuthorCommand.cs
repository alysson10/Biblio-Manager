using Bib.Application.Common.Models;
using MediatR;

namespace Bib.Application.Features.Author.Command.DeleteAuthor
{
    public class DeleteAuthorCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
}
