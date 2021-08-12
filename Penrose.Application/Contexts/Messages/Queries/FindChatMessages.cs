using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Penrose.Application.Common;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.Interfaces.ChatStrategies;
using Penrose.Application.Interfaces;

namespace Penrose.Application.Contexts.Messages.Queries
{
    public class FindChatMessagesRequest : PagedRequest, IRequest<PagedResult<ChatMessageDto>>
    {
        public Guid ChatId { get; set; }
        public PagedRequest PagedRequest { get; set; }

        public class FindChatMessageRequestHandler : IRequestHandler<FindChatMessagesRequest, PagedResult<ChatMessageDto>>
        {
            private readonly IChatDataStrategy _chatDataStrategy;
            private readonly ISecurityService _securityService;

            public FindChatMessageRequestHandler(IChatDataStrategy chatDataStrategy, ISecurityService securityService)
            {
                _chatDataStrategy = chatDataStrategy;
                _securityService = securityService;
            }

            public async Task<PagedResult<ChatMessageDto>> Handle(
                FindChatMessagesRequest request,
                CancellationToken cancellationToken)
            {
                Guid currentUserId = _securityService.GetCurrentUserId();
                Guid chatId = request.ChatId;

                await CheckIfUserBelongsToChatAsync(currentUserId, chatId, cancellationToken);
                int messageCount = await _chatDataStrategy.CountChatMessagesAsync(request.ChatId, cancellationToken);
                IEnumerable<ChatMessageDto> pagedResult = await _chatDataStrategy.FindChatMessagesAsync(
                    request.ChatId,
                    request.PagedRequest,
                    cancellationToken);

                return new PagedResult<ChatMessageDto>()
                {
                    Count = messageCount,
                    Offset = request.Offset,
                    PageSize = request.PageSize,
                    Records = pagedResult,
                };
            }

            private async Task CheckIfUserBelongsToChatAsync(
                Guid currentUserId,
                Guid chatId,
                CancellationToken cancellationToken)
            {
                bool userBelongsToChat = await _chatDataStrategy.UserBelongsToChatAsync(
                    currentUserId,
                    chatId,
                    cancellationToken);

                if (!userBelongsToChat)
                    throw new UnauthorizedAccessException();
            }
        }
    }
}