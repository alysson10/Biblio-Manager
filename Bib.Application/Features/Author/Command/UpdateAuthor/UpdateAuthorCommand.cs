using Bib.Application.Common.Models;
using MediatR;

namespace Bib.Application.Features.Author.Command.UpdateAuthor
{
    public class UpdateAuthorCommand : IRequest<Result<int>>
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
        /// Status
        /// </summary>
        public bool Status { get; set; }
    }
}
