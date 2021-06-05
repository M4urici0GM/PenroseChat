using System;

namespace Penrose.Application.DataTransferObjects
{
    public class ChatMessageDto
    {
        public Guid ChatId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
        public bool IsSeen { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DateSeen { get; set; }
        public DateTime? DateDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public string From { get; set; }
    }
}