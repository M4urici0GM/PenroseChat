using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Penrose.Core.Entities;

namespace Penrose.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.LastName).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Hash).IsRequired();
            builder.Property(x => x.LastLogin).IsRequired();
            builder.Property(x => x.Is2FaEnabled).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.IsEmailVerified).IsRequired().HasDefaultValue(false);
        }
    }
}