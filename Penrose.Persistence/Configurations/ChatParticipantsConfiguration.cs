﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Penrose.Core.Entities;

namespace Penrose.Persistence.Configurations
{
    public class ChatParticipantsConfiguration : EntityConfiguration<ChatParticipants>
    {
        protected override void InternalConfiguration(EntityTypeBuilder<ChatParticipants> builder)
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