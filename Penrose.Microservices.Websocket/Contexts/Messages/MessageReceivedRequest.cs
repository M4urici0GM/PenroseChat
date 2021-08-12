using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Penrose.Microservices.Websocket.Services;

namespace Penrose.Microservices.Websocket.Contexts.Messages
{

    public class UserMessageDto
    {
        public Guid To { get; set; }
        public Guid CurrentConnectionId { get; set; }
        public string Content { get; set; }
    }

    public class MessageReceivedRequest : IRequest
    {
        public byte[] MessageBuffer { get; set; }
        public DateTime ReceivedAt { get; set; }
        
        public class MessageReceivedRequestHandler : IRequestHandler<MessageReceivedRequest>
        {
            private readonly IWebsocketManager _websocketManager;
            
            public MessageReceivedRequestHandler(IWebsocketManager websocketManager)
            {
                _websocketManager = websocketManager;
            }
            
            public Task<Unit> Handle(MessageReceivedRequest request, CancellationToken cancellationToken)
            {
                string messageBufferStr = Encoding.UTF8.GetString(request.MessageBuffer);
                UserMessageDto messageDto = JsonConvert.DeserializeObject<UserMessageDto>(messageBufferStr);
                if (messageDto == null)
                    throw new InvalidOperationException("");
                
                bool isDestinationUserConnected = _websocketManager.UserHasConnection(messageDto.To);
                if (!isDestinationUserConnected)
                    return null;

                return null;
            }
        }
    }
}