using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Penrose.Application.Common;
using Penrose.Application.DataTransferObjects;
using Penrose.Core.Entities;

namespace Penrose.Application.Interfaces.ChatStrategies
{
  public interface IChatDataStrategy
  {
    Task<IEnumerable<ChatDto>> FindUserChatsAsync(Guid userId, CancellationToken cancellationToken);
    Task<bool> UserBelongsToChatAsync(Guid userId, Guid chatId, CancellationToken cancellationToken);
    Task<int> CountChatMessagesAsync(Guid chatId, CancellationToken cancellationToken);
    Task<int> CountUserChatsAsync(Guid userId, CancellationToken cancellationToken);
    Task<Chat> InsertOneAsync(Chat chat, CancellationToken cancellationToken);
    Task<IEnumerable<ChatMessageDto>> FindChatMessagesAsync(
        Guid chatId,
        PagedRequest pagedRequest,
        CancellationToken cancellationToken);
  }
}