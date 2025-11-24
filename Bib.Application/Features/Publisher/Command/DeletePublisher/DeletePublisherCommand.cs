using Bib.Application.Common.Models;
using MediatR;

namespace Bib.Application.Features.Publisher.Command.DeletePublisher
{
    public record DeletePublisherCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
}
