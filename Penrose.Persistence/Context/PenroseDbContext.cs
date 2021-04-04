using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Penrose.Core.Exceptions;
using Penrose.Core.Interfaces;

namespace Penrose.Persistence.Context
{
    public class PenroseDbContext : DbContext, IPenroseDbContext
    {
        public PenroseDbContext(DbContextOptions<PenroseDbContext> options): base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChanges();
            
            ChangeTracker.Entries<IEntity>()
                .ToList()
                .ForEach(entity =>
                {
                    PropertyEntry<IEntity,Guid> versionProperty = entity.Property(x => x.Version);
                    PropertyEntry<IEntity, bool> isActiveProperty = entity.Property(x => x.IsActive);
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
    }
}