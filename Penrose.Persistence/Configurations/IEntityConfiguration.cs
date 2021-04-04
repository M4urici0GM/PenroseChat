using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Penrose.Core.Interfaces;

namespace Penrose.Persistence.Configurations
{
    public class EntityConfiguration : IEntityTypeConfiguration<IEntity>
    {
        public void Configure(EntityTypeBuilder<IEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Version).ValueGeneratedOnAdd();
            builder.Property(x => x.UpdatedAt).ValueGeneratedOnAddOrUpdate();
            builder.Property(x => x.CreatedAt).ValueGeneratedOnAdd();
        }
    }
}