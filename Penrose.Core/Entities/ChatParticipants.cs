using System;
using Penrose.Core.Interfaces;

namespace Penrose.Core.Entities
{
    public class ChatParticipants : IEntity
    {
        public Guid Id { get; set; }
        public Guid Version { get; set; }
        public Guid UserId { get; set; }
        public Guid ChatId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public User User { get; set; }
        public Chat Chat { get; set; }
    }
}