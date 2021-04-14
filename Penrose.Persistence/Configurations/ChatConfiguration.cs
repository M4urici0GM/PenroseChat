using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Penrose.Core.Entities;

namespace Penrose.Persistence.Configurations
{
    public class ChatConfiguration : EntityConfiguration<Chat>
    {
        protected override void InternalConfiguration(EntityTypeBuilder<Chat> builder)
        {
            builder.HasMany(x => x.Messages)
                .WithOne(x => x.Chat)
                .HasForeignKey(x => x.ChatId);

            builder.HasMany(x => x.Participants)
                .WithOne(x => x.Chat)
                .HasForeignKey(x => x.ChatId);
        }
    }
}