using Penrose.Core.Common;
using Penrose.Core.Interfaces;
using Penrose.Core.Interfaces.Strategies;

namespace Penrose.Application.Strategies
{
    public class DataStrategy<TEntity> : IDataStrategy<TEntity> where TEntity : AuditableEntity
    {
        protected IPenroseDbContext PenroseDbContext { get; }
        
        public DataStrategy(IPenroseDbContext dbContext)
        {
            PenroseDbContext = dbContext;
        }
    }
}