using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Penrose.Application.Common;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.Extensions;
using Penrose.Application.Interfaces;
using Penrose.Application.Interfaces.ChatStrategies;

namespace Penrose.Application.Contexts.Chats.Queries
{
    public class GetUserChatsRequest : PagedRequest, IRequest<PagedResult<ChatDto>>
    {
        public class GetUserChatsRequestHandler : IRequestHandler<GetUserChatsRequest, PagedResult<ChatDto>>
        {
            private readonly IChatDataStrategy _chatDataStrategy;
            private readonly ISecurityService _securityService;

            public GetUserChatsRequestHandler(IChatDataStrategy chatDataStrategy, ISecurityService securityService)
            {
                _chatDataStrategy = chatDataStrategy;
                _securityService = securityService;
            }

            public async Task<PagedResult<ChatDto>> Handle(GetUserChatsRequest request, CancellationToken cancellationToken)
            {
                Guid userId = _securityService.GetCurrentUserId();
                int chatCount = await _chatDataStrategy.CountUserChatsAsync(userId, cancellationToken);
                IEnumerable<ChatDto> userChats = await _chatDataStrategy.FindUserChatsAsync(userId, cancellationToken);

                return new PagedResult<ChatDto>()
                {
                    Count = chatCount,
                    Offset = request.Offset,
                    PageSize = request.PageSize,
                    Records = userChats,
                };
            }
        }
    }
}