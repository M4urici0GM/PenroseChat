using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Penrose.Core.Common;

namespace Penrose.Persistence.Configurations
{
    public abstract class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : AuditableEntity
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Version).ValueGeneratedOnAdd();
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.UpdatedAt)
                .HasDefaultValueSql("getdate()")
                .ValueGeneratedOnAddOrUpdate();

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("getdate()")
                .ValueGeneratedOnAdd();

            InternalConfiguration(builder);
        }

        protected abstract void InternalConfiguration(EntityTypeBuilder<TEntity> builder);
    }
}