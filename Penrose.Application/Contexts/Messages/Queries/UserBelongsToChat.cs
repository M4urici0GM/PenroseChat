using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Penrose.Application.Interfaces.ChatStrategies;

namespace Penrose.Application.Contexts.Messages.Queries
{
    public class UserBelongsToChat : IRequest<bool>
    {
        public Guid UserId { get; }
        public Guid ChatId { get; }

        public UserBelongsToChat(Guid userId, Guid chatId)
        {
            UserId = userId;
            ChatId = chatId;
        }

        public class UserBelongsToChatRequestHandler : IRequestHandler<UserBelongsToChat, bool>
        {
            private readonly IChatDataStrategy _chatDataStrategy;
            
            public UserBelongsToChatRequestHandler(IChatDataStrategy chatDataStrategy)
            {
                _chatDataStrategy = chatDataStrategy;
            }
            
            
            public async Task<bool> Handle(UserBelongsToChat request, CancellationToken cancellationToken)
            {
                Guid currentUserId = request.UserId;
                Guid chatId = request.ChatId;
                
                bool userBelongsToChat = await _chatDataStrategy.UserBelongsToChatAsync(
                    currentUserId,
                    chatId,
                    cancellationToken);

                return userBelongsToChat;
            }
        }
    }
}