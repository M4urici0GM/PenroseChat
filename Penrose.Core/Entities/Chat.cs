using System;
using System.Collections.Generic;
using Penrose.Core.Interfaces;

namespace Penrose.Core.Entities
{
    public class Chat : IEntity
    {
        public Guid Id { get; set; }
        public Guid Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        public IEnumerable<ChatParticipants> Participants { get; set; }
        public IEnumerable<ChatMessage> Messages { get; set; }
    }
}