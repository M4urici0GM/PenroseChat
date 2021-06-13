using System;
using System.Collections.Generic;
using Penrose.Core.Common;

namespace Penrose.Core.Entities
{
    public class User : AuditableEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string Hash { get; set; }
        public bool Is2FaEnabled { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime LastLogin { get; set; }

        public IEnumerable<Chat> Chats { get; set; }
        public UserProperties Properties { get; set; }
    }
}