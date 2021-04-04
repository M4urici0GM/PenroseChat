using System;

namespace Penrose.Core.Interfaces
{
    public interface IEntity
    {
        Guid Id { get; set; }
        Guid Version { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
        bool IsActive { get; set; }
    }
}