using System.Threading.Tasks;
using MediatR;
using Penrose.Infrastructure.EventBus.Attributes;
using Penrose.Infrastructure.EventBus.Interfaces;

namespace Penrose.Application.Consumers
{
    [BindToExchange("penrose.chat.messages")]
    public class UserMessageConsumer : IEventConsumer
    {
        private readonly IMediator _mediator;
        
        public UserMessageConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public void HandleEvent(string eventName, byte[] payload)
        {
            throw new System.NotImplementedException();
        }

        public Task HandleEventAsync(string eventName, byte[] payload)
        {
            throw new System.NotImplementedException();
        }
    }
}