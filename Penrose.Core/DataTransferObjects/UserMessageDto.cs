using System;

namespace Penrose.Core.DataTransferObjects
{
    public class UserMessageDto
    {
        public Guid From { get; set; }
        public Guid To { get; set; }
        public Guid CurrentConnectionId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}