using System;

namespace Penrose.Application.DataTransferObjects
{
    public class ChatParticipantDto
    {
        public Guid Id { get; set; }
        public UserDto User { get; set; }
    }
}