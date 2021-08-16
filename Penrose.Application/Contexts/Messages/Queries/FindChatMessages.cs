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
            private readonly IMediator _mediator;
            
            public FindChatMessageRequestHandler(
                IChatDataStrategy chatDataStrategy,
                ISecurityService securityService,
                IMediator mediator)
            {
                _chatDataStrategy = chatDataStrategy;
                _securityService = securityService;
                _mediator = mediator;
            }

            public async Task<PagedResult<ChatMessageDto>> Handle(
                FindChatMessagesRequest request,
                CancellationToken cancellationToken)
            {
                Guid userId = _securityService.GetCurrentUserId();
                Guid chatId = request.ChatId;

                await CheckIfUserBelongsToChatAsync(new UserBelongsToChat(userId, chatId), cancellationToken);
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

            private async Task CheckIfUserBelongsToChatAsync(UserBelongsToChat request, CancellationToken cancellationToken)
            {
                bool userBelongsToChat = await _mediator.Send(request, cancellationToken);
                if (!userBelongsToChat)
                    throw new UnauthorizedAccessException();
            }
        }
    }
}