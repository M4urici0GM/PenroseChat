using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Penrose.Core.Common;
using Penrose.Core.Interfaces;

namespace Penrose.Persistence.Strategies
{
    public class MssqlDataStrategy<TEntity> : IMssqlDataStrategy<TEntity> where TEntity : AuditableEntity
    {
        public MssqlDataStrategy(IPenroseDbContext dbContext)
        {
            PenroseDbContext = dbContext;
        }

        public IPenroseDbContext PenroseDbContext { get; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return PenroseDbContext.SaveChangesAsync(cancellationToken);
        }

        public DbSet<TEntity> GetDbSet()
        {
            return PenroseDbContext.GetDbSet<TEntity>();
        }

        public IPenroseDbContext GetContext()
        {
            return PenroseDbContext;
        }
    }
}