namespace Penrose.Infrastructure.EventBus.Interfaces
{
    public interface IRabbitMqEventBus
    {
        void Subscribe<T>() where T : IEventConsumer;
        void Publish<T>(T @event) where T : IEvent;
    }
}