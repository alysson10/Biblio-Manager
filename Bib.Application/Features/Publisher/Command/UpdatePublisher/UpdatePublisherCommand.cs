using Bib.Application.Common.Models;
using MediatR;

namespace Bib.Application.Features.Publisher.Command.UpdatePublisher
{
    public class UpdatePublisherCommand : IRequest<Result<int>>
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

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

        /// <summary>
        /// Status
        /// </summary>        
        public bool Status { get; set; } = true;
    }
}
