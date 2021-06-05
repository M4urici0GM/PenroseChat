using System;
using Penrose.Core.Common;

namespace Penrose.Core.Entities
{
    public class ChatParticipant : AuditableEntity
    {
        public Guid UserId { get; set; }
        public Guid ChatId { get; set; }
        public User User { get; set; }
        public Chat Chat { get; set; }
    }
}