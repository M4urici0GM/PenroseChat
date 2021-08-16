using Penrose.Infrastructure.EventBus.Attributes;
using Penrose.Infrastructure.EventBus.Interfaces;

namespace Penrose.Infrastructure.BusEvents
{
    [BindToExchange("penrose.chat.messages")]
    public class UserMessageBusEvent : IEvent
    {
        public byte[] Buffer { get; set; }
        public UserMessageBusEvent(byte[] buffer)
        {
            Buffer = buffer;
        }
    }
}