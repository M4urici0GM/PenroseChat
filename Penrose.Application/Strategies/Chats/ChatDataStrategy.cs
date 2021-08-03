using System;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Penrose.Core.Interfaces;
using Penrose.Core.Entities;
using Penrose.Application.Common;
using Penrose.Application.Extensions;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.Interfaces.ChatStrategies;

namespace Penrose.Application.Strategies.Chats
{
    public class ChatDataStrategy : DataStrategy<Chat>, IChatDataStrategy
    {
        private readonly DbSet<Chat> _chatDb;
        private readonly DbSet<ChatMessage> _chatMessageDb;
        private readonly IMapper _mapper;
        
        public ChatDataStrategy(
            IMapper mapper,
            IPenroseDbContext dbContext) : base(dbContext)
        {
            _chatDb = dbContext.GetDbSet<Chat>();
            _chatMessageDb = dbContext.GetDbSet<ChatMessage>();
            _mapper = mapper;
        }

        public Task<int> CountChatMessagesAsync(Guid chatId, CancellationToken cancellationToken)
        {
            return _chatMessageDb
                .Where(x => x.ChatId == chatId)
                .CountAsync(cancellationToken);
        }

        public Task<int> CountUserChatsAsync(Guid userId, CancellationToken cancellationToken)
        {
            return _chatDb
                .Include(x => x.Participants)
                .Where(x => x.Participants
                    .Any(chatParticipant => chatParticipant.UserId == userId))
                .CountAsync(cancellationToken);
        }

        public async Task<IEnumerable<ChatMessageDto>> FindChatMessagesAsync(
            Guid chatId,
            PagedRequest pagedRequest,
            CancellationToken cancellationToken)
        {
            Dictionary<string, Expression<Func<ChatMessage, object>>> availableOrderings =
                new() {{"created", (message => message.CreatedAt)}};

            List<ChatMessageDto> chatMessages = await _chatMessageDb
                .Where(x => x.ChatId == chatId)
                .Include(x => x.User)
                .ApplyPagination(pagedRequest)
                .Select(chatMessage => new ChatMessageDto() {
                    ChatId = chatMessage.ChatId,
                    Content = chatMessage.Content,
                    ContentType = chatMessage.ContentType,
                    DateDeleted = chatMessage.DateDeleted,
                    DateSeen = chatMessage.DateDeleted,
                    IsDeleted = chatMessage.IsDeleted,
                    IsSeen = chatMessage.IsSeen,
                    UserId = chatMessage.UserId,
                    From = chatMessage.User.Name,
                    CreatedAt = chatMessage.CreatedAt,
                })
                .ToListAsync(cancellationToken);

            return chatMessages
                .OrderByDescending(x => x.CreatedAt);
        }

        public async Task<IEnumerable<ChatDto>> FindUserChatsAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _chatDb
                .Include(chat => chat.Properties)
                .Include(chat => chat.Participants)
                .ThenInclude(chat => chat.User)
                .Include(chat => chat.Messages)
                .ThenInclude(chatMessage => chatMessage.User)
                .Where(x => x.Participants.Any(chatParticipant => chatParticipant.UserId == userId))
                .Select(chat => new ChatDto()
                {
                    Id = chat.Id,
                    Properties = chat.Properties,
                    UnreadMessages = chat.Messages.Count(x => !x.IsSeen),
                    CreatedAt = chat.CreatedAt,
                    UpdatedAt = chat.UpdatedAt,
                    Participants = chat.Participants
                        .Select(chatParticipant => _mapper.Map<UserDto>(chatParticipant.User)),
                    LastMessage = chat.Messages
                        .OrderByDescending(chatMessage => chatMessage.CreatedAt)
                        .Where(chatMessage => !chatMessage.IsDeleted)
                        .Select(chatMessage => new ChatMessageDto
                        {
                            Content = chatMessage.Content,
                            ChatId = chat.Id,
                            ContentType = chatMessage.ContentType,
                            DateDeleted = chatMessage.DateDeleted,
                            DateSeen = chatMessage.DateSeen,
                            IsDeleted = chatMessage.IsDeleted,
                            IsSeen = chatMessage.IsSeen,
                            UserId = chatMessage.UserId,
                            From = chatMessage.User.Name,
                        })
                        .FirstOrDefault()
                })
                .ToListAsync(cancellationToken);
        }

        public Task<bool> UserBelongsToChatAsync(Guid userId, Guid chatId, CancellationToken cancellationToken)
        {
            return _chatDb
                .Where(x => x.Id == chatId)
                .Where(x => x.Participants.Any(chatParticipant => chatParticipant.UserId == userId))
                .AnyAsync(cancellationToken);
        }
    }
}