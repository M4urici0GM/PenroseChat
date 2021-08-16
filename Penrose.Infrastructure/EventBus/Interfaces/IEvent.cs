namespace Penrose.Infrastructure.EventBus.Interfaces
{
    public interface IEvent
    {
        public byte[] Buffer { get; set; }
    }
}