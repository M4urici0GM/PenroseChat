using System.Threading;
using System.Threading.Tasks;

namespace Penrose.Infrastructure.EventBus.Interfaces
{
    public interface IEventConsumer
    {
        void HandleEvent(string eventName, byte[] payload);
        Task HandleEventAsync(string eventName, byte[] payload);
    }
}