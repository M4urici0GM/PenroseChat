using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Penrose.Core.Common;

namespace Penrose.Core.Interfaces
{
  public interface IPenroseDbContext
  {
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
    DbSet<T> GetDbSet<T>() where T : AuditableEntity;
    EntityEntry GetEntityEntry<T>(T entity);
    EntityEntry AttachEntity<T>(T entity);
  }
}