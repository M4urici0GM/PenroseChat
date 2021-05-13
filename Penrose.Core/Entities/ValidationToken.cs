using System;
using Penrose.Core.Common;

namespace Penrose.Core.Entities
{
    public class ValidationToken
    {
        public string Id { get; set; }
        public int Code { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}