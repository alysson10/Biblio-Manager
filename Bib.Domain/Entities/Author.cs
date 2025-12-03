using System;

namespace Bib.Domain.Entities
{
    public sealed class Author
    {
        public int Id { get; internal set; }
        public int UserId { get; internal set; }
        public string Name { get; internal set; } = string.Empty;
        public string? Email { get; internal set; }
        public string? PhoneNumber { get; internal set; }
        public bool Status { get; internal set; } = true;
        public DateTime CreatedAt { get; internal set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; internal set; }
        public DateTime? DeletedAt { get; internal set; }

        private Author() { }

        public Author(int userId, string name, string? email, string? phoneNumber, bool status, DateTime? updatedAt = null)
        {
            Id = userId;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Status = status;
            UpdatedAt = updatedAt is null ? null : updatedAt;
        }

        public void UpdatePublisher(string name, string? email, string? phoneNumber, bool status)
        {
            Name = name;
            PhoneNumber = phoneNumber is null ? null : phoneNumber;
            Email = email is null ? null : email;
            Status = status;
            UpdatedAt = DateTime.Now;
        }
    }
}
