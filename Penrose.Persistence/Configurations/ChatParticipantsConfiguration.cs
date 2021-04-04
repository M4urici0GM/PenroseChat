using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Penrose.Core.Entities;

namespace Penrose.Persistence.Configurations
{
    public class ChatParticipantsConfiguration : IEntityTypeConfiguration<ChatParticipants>
    {
        public void Configure(EntityTypeBuilder<ChatParticipants> builder)
        {
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Chat)
                .WithMany(x => x.Participants)
                .HasForeignKey(x => x.ChatId);
        }
    }
}