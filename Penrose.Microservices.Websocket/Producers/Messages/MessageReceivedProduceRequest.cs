using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Penrose.Core.DataTransferObjects;
using Penrose.Infrastructure.BusEvents;
using Penrose.Infrastructure.EventBus.Interfaces;
using Penrose.Microservices.Websocket.Services;

namespace Penrose.Microservices.Websocket.Producers.Messages
{
    public class MessageReceivedProduceRequest : IRequest
    {
        public byte[] MessageBuffer { get; set; }
        public WebsocketConnection WebsocketIdentifier { get; set; }
        public DateTime ReceivedAt { get; }

        public MessageReceivedProduceRequest()
        {
            ReceivedAt = DateTime.UtcNow;
        }

        public class MessageReceivedRequestHandler : IRequestHandler<MessageReceivedProduceRequest>
        {
            private readonly IWebsocketManager _websocketManager;
            private readonly IRabbitMqEventBus _eventBus;
            
            public MessageReceivedRequestHandler(
                IWebsocketManager websocketManager,
                IRabbitMqEventBus eventBus)
            {
                _websocketManager = websocketManager;
                _eventBus = eventBus;
            }
            
            public Task<Unit> Handle(MessageReceivedProduceRequest produceRequest, CancellationToken cancellationToken)
            {
                string messageBufferStr = Encoding.UTF8.GetString(produceRequest.MessageBuffer);
                UserMessageDto messageDto = JsonConvert.DeserializeObject<UserMessageDto>(messageBufferStr);
                if (messageDto == null)
                    throw new InvalidOperationException("");
                
                // TODO: Validate the message content.
                
                _eventBus.Publish(new UserMessageBusEvent(produceRequest.MessageBuffer));
                return null;
            }
        }
    }
}