using System;
using Penrose.Core.Common;

namespace Penrose.Core.Entities
{
    public class ChatProperties : AuditableEntity
    {
        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}