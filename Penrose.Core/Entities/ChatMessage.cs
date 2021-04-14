using System;
using Penrose.Core.Common;

namespace Penrose.Core.Entities
{
    public class ChatMessage : AuditableEntity
    {
        public Guid ChatId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
        public bool IsSeen { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DateSeen { get; set; }
        public DateTime? DateDeleted { get; set; }
        public User User { get; set; }
        public Chat Chat { get; set; }
    }
}