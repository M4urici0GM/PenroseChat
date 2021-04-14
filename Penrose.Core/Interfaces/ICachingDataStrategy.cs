using System;
using System.Threading.Tasks;
using Penrose.Core.Common;

namespace Penrose.Core.Interfaces
{
    public interface ICachingDataStrategy<TEntity> : IDataStrategy<TEntity> where TEntity : AuditableEntity
    {
        Task<bool> Set(TEntity entity);
        Task<TEntity> Get(Guid entityId);
    }
}