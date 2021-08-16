using System;
using System.Threading.Tasks;
using MediatR;
using Penrose.Infrastructure.EventBus.Attributes;
using Penrose.Infrastructure.EventBus.Interfaces;

namespace Penrose.Microservices.Websocket.Consumers
{
    [BindToExchange("penrose.chat.notifications")]
    public class WebsocketNotificationConsumer : IEventConsumer
    {
        private readonly IMediator _mediator;
        
        public WebsocketNotificationConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void HandleEvent(string eventName, byte[] payload)
        {
            throw new NotImplementedException();
        }

        public Task HandleEventAsync(string eventName, byte[] payload)
        {
            

            return Task.CompletedTask;
        }
    }
}