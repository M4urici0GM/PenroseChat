using System;
using System.Collections.Generic;
using Penrose.Core.Entities;

namespace Penrose.Application.DataTransferObjects
{
    public class ChatDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UnreadMessages { get; set; }
        public IEnumerable<UserDto> Participants { get; set; }
        public ChatMessageDto LastMessage { get; set; }
        public ChatPropertiesDto Properties { get; set; }
    }
}