using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Penrose.Core.Entities;

namespace Penrose.Persistence.Configurations
{
    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.ContentType).IsRequired();
            builder.Property(x => x.IsSeen).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);

            builder.HasOne(x => x.Chat)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.ChatId);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }
    }
}