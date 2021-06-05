using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Penrose.Application.Common;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.Extensions;
using Penrose.Application.Interfaces.ChatStrategies;

namespace Penrose.Application.Contexts.Chats.Queries
{
    public class GetUserChatsRequest : PagedRequest, IRequest<PagedResult<ChatDto>>
    {
        public class GetUserChatsRequestHandler : IRequestHandler<GetUserChatsRequest, PagedResult<ChatDto>>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IChatDataStrategy _chatDataStrategy;

            public GetUserChatsRequestHandler(IHttpContextAccessor httpContextAccessor, IChatDataStrategy chatDataStrategy)
            {
                _httpContextAccessor = httpContextAccessor;
                _chatDataStrategy = chatDataStrategy;
            }

            public async Task<PagedResult<ChatDto>> Handle(GetUserChatsRequest request, CancellationToken cancellationToken)
            {
                Guid userId = _httpContextAccessor.GetUserId();
                int chatCount = await _chatDataStrategy.CountUserChatsAsync(userId, cancellationToken);
                IEnumerable<ChatDto> userChats = await _chatDataStrategy.FindUserChatsAsync(userId, cancellationToken);

                return new PagedResult<ChatDto>()
                {
                    Count = chatCount,
                    Offset = request.Offset,
                    Pagesize = request.Pagesize,
                    Records = userChats,
                };
            }
        }
    }
}