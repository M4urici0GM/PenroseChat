using Penrose.Core.Common;

namespace Penrose.Application.Interfaces.Strategies
{
    public interface IDataStrategy<TEntity> where TEntity : AuditableEntity
    {
    }
}