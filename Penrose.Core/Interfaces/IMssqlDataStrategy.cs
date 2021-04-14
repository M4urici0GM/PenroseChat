using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Penrose.Core.Common;

namespace Penrose.Core.Interfaces
{
    public interface IMssqlDataStrategy<TEntity> : IDataStrategy<TEntity> where TEntity : AuditableEntity
    {
        IPenroseDbContext PenroseDbContext { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DbSet<TEntity> GetDbSet();
        IPenroseDbContext GetContext();
    }
}