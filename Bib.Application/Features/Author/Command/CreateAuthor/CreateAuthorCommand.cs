using Bib.Application.Common.Models;
using MediatR;

namespace Bib.Application.Features.Author.Command.CreateAuthor
{
    public record CreateAuthorCommand : IRequest<Result<int>>
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// PhoneNumber
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public bool Status { get; set; } = true;
    }
}
