using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Penrose.Core.Entities;

namespace Penrose.Persistence.Configurations
{
    public class ChatPropertiesConfiguration : EntityConfiguration<ChatProperties>
    {
        protected override void InternalConfiguration(EntityTypeBuilder<ChatProperties> builder)
        {

        }
    }
}