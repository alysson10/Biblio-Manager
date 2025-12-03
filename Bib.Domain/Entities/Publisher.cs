using System;

namespace Bib.Domain.Entities
{
    public sealed class Publisher
    {
        public int Id { get; internal set; }
        public int UserId { get; internal set; }
        public string Name { get; internal set; } = string.Empty;
        public string? Description { get; internal set; }
        public string? PhoneNumber { get; internal set; }
        public string? Email { get; internal set; }
        public string? Site { get; internal set; }
        public bool Status { get; internal set; } = true;
        public DateTime CreatedAt { get; internal set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; internal set; }

        private Publisher()
        {
            
        }

        public Publisher(int userId, string name, string? description, string? phoneNumber, string? email, 
                         string? site, bool status, DateTime? updatedAt = null)
        {
            Id = userId;
            Name = name;
            Description = description;
            PhoneNumber = phoneNumber;
            Email = email;
            Site = site;
            Status = status;
            UpdatedAt = updatedAt is null ? null : updatedAt;
        }

        public void UpdatePublisher(string name, string? description, string? phoneNumber, string? email,
                                    string? site, bool status)
        {            
            Name = name;
            Description = description is null ? null : description;
            PhoneNumber = phoneNumber is null ? null : phoneNumber;
            Email = email is null ? null : email;
            Site= site is null ? null : site;
            Status = status;
            UpdatedAt = DateTime.Now;
        }
    }
}
