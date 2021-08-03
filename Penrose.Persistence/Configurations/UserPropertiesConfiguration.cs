using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Penrose.Core.Entities;

namespace Penrose.Persistence.Configurations
{
    public class UserPropertiesConfiguration : EntityConfiguration<UserProperties>
    {
        protected override void InternalConfiguration(EntityTypeBuilder<UserProperties> builder)
        {}
    }
}