using System;
using System.Collections.Generic;
using Penrose.Core.Interfaces;

namespace Penrose.Core.Entities
{
    public class ChatMessage : IEntity
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
        public bool IsSeen { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DateSeen { get; set; }
        public DateTime? DateDeleted { get; set; }
        public Guid Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        public User User { get; set; }
        public Chat Chat { get; set; }
    }
}