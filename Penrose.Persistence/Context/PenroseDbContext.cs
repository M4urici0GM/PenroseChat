using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Penrose.Core.Common;
using Penrose.Core.Exceptions;
using Penrose.Core.Interfaces;

namespace Penrose.Persistence.Context
{
    public class PenroseDbContext : DbContext, IPenroseDbContext
    {
        public PenroseDbContext(DbContextOptions options) : base(options)
        {
            
        }
        
        public DbSet<T> GetDbSet<T>() where T : AuditableEntity
        {
            return base.Set<T>();
        }

        public EntityEntry GetEntityEntry<T>(T entity)
        {
            return base.Entry(entity);
        }

        public EntityEntry AttachEntity<T>(T entity)
        {
            return base.Attach(entity);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            ChangeTracker.DetectChanges();

            ChangeTracker.Entries<AuditableEntity>()
                .ToList()
                .ForEach(entity =>
                {
                    var versionProperty = entity.Property(x => x.Version);
                    var isActiveProperty = entity.Property(x => x.IsActive);
                    if (versionProperty.OriginalValue != versionProperty.CurrentValue)
                        throw new ConcurrencyException();

                    if (entity.State == EntityState.Deleted)
                    {
                        entity.State = EntityState.Modified;
                        isActiveProperty.CurrentValue = false;
                    }

                    versionProperty.CurrentValue = Guid.NewGuid();
                });

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}