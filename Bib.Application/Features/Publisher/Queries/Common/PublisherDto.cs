using System;

namespace Bib.Application.Features.Publisher.Queries.Common
{
    public record PublisherDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Site { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
