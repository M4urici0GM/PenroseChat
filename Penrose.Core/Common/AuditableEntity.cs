using System;
using Newtonsoft.Json;

namespace Penrose.Core.Common
{
    public abstract class AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}