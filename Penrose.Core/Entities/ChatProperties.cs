using System;
using Penrose.Core.Common;
using Penrose.Core.Enums;

namespace Penrose.Core.Entities
{
    public class ChatProperties : AuditableEntity
    {
        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }
        public string Name { get; set; }
        public ChatType Type { get; set; }
        public string PhotoUrl { get; set; }
        public bool IsMuted { get; set; }
        public bool IsPinned { get; set; }
    }
}