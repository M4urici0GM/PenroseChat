using System;
using System.Collections.Generic;
using Penrose.Core.Common;

namespace Penrose.Core.Entities
{
    public class User : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid Version { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Hash { get; set; }
        public bool Is2FaEnabled { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        public IEnumerable<Chat> Chats { get; set; }
    }
}