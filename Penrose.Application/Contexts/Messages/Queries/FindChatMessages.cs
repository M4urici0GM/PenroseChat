using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Penrose.Application.Common;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.Interfaces.ChatStrategies;
using Penrose.Application.Extensions;

namespace Penrose.Application.Contexts.Messages.Queries
{
  public class FindChatMessagesRequest : PagedRequest, IRequest<PagedResult<ChatMessageDto>>
  {
    public Guid ChatId { get;set; }
    public PagedRequest PagedRequest { get; set; }

    public class FindChatMessageRequestHandler : IRequestHandler<FindChatMessagesRequest, PagedResult<ChatMessageDto>> {

      private readonly IHttpContextAccessor _httpContextAccessor;
      private readonly IChatDataStrategy _chatDataStrategy;

      public FindChatMessageRequestHandler(IHttpContextAccessor contextAccessor, IChatDataStrategy chatDataStrategy)
      {
          _httpContextAccessor = contextAccessor;
          _chatDataStrategy = chatDataStrategy;
      }

      public async Task<PagedResult<ChatMessageDto>> Handle(FindChatMessagesRequest request, CancellationToken cancellationToken)
      {
        Guid currentUserId = _httpContextAccessor.GetUserId();
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
          Pagesize = request.Pagesize,
          Records = pagedResult,
        };
      }

      private async Task CheckIfUserBelongsToChatAsync(
        Guid currentUserId,
        Guid chatId,
        CancellationToken cancellationToken)
      {
        bool userBelongsToChat = await _chatDataStrategy.UserBelongsToChatAsync(currentUserId, chatId, cancellationToken);
        if (!userBelongsToChat)
          throw new UnauthorizedAccessException();
      }
    }
  }
}