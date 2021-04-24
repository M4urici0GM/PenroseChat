using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Penrose.Core.Common;
using Penrose.Core.Interfaces;

namespace Penrose.Persistence.Strategies
{
    public class DataStrategy<TEntity> : IDataStrategy<TEntity> where TEntity : AuditableEntity
    {
        private IPenroseDbContext PenroseDbContext { get; }
        
        public DataStrategy(IPenroseDbContext dbContext)
        {
            PenroseDbContext = dbContext;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
            PenroseDbContext.SaveChangesAsync(cancellationToken);

        public DbSet<TEntity> GetDbSet() =>
            PenroseDbContext.GetDbSet<TEntity>();

        public IPenroseDbContext GetContext() =>
            PenroseDbContext;
    }
}