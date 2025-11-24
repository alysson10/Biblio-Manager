using Bib.Application.Common.Models;
using MediatR;

namespace Bib.Application.Features.Publisher.Command.CreatePublisher
{
    public record CreatePublisherCommand : IRequest<Result<int>>
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// PhoneNumber
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Site
        /// </summary>
        public string? Site { get; set; }        
    }
}
