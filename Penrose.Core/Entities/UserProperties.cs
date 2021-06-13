using System;
using Penrose.Core.Common;

namespace Penrose.Core.Entities
{
    public class UserProperties : AuditableEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        public string PhotoUrl { get; set; }
        public string Status { get; set; }
        public DateTime LastSeen { get; set; }
    }
}