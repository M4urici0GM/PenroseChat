
using System;
using MediatR;
using System.Collections.Generic;
using Penrose.Application.DataTransferObjects;
using System.Threading.Tasks;
using System.Threading;
using Penrose.Application.Interfaces.ChatStrategies;
using Penrose.Application.Contexts.Chats.Validators;
using Penrose.Application.Extensions;
using AutoMapper;
using Penrose.Core.Entities;
using System.Linq;
using Penrose.Application.Interfaces.UserStrategies;
using Penrose.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Penrose.Application.Interfaces;

namespace Penrose.Application.Contexts.Chats.Commands
{
  public class CreateChatRequest : IRequest<ChatDto>
  {
    public ChatPropertiesDto ChatProperties { get; set; }
    public IEnumerable<Guid> Participants { get; set; }

    public class CreateChatRequestHandler : IRequestHandler<CreateChatRequest, ChatDto>
    {
      private readonly IChatDataStrategy _chatDataStrategy;
      private readonly IUserDataStrategy _userDataStrategy;
      private readonly ILogger<CreateChatRequestHandler> _logger;
      private readonly ISecurityService  _securityService;
      private readonly IMapper _mapper;

      public CreateChatRequestHandler(
        IMapper mapper,
        IChatDataStrategy chatDataStrategy,
        IUserDataStrategy userDataStrategy,
        ISecurityService securityService,
        ILogger<CreateChatRequestHandler> logger)
      {
        _mapper = mapper;
        _logger = logger;
        _securityService = securityService;
        _chatDataStrategy = chatDataStrategy;
        _userDataStrategy = userDataStrategy;
      }

      public async Task<ChatDto> Handle(CreateChatRequest request, CancellationToken cancellationToken)
      {
        await ValidateRequestAsync(request, cancellationToken);
        await ValidateParticipantsAsync(request, cancellationToken);

        Guid currentUserId = _securityService.GetCurrentUserId();
        List<ChatParticipant> participants = request.Participants
          .Where(userId => currentUserId != userId)
          .Select(userId => new ChatParticipant() {UserId = userId})
          .ToList();
        
        participants.Add(new ChatParticipant() { UserId = currentUserId });
        
        var chat = new Chat()
        {
          Participants = participants,
          Properties = new ChatProperties()
          {
            IsActive = true,
            IsMuted = false,
            IsPinned = false,
            Type = request.ChatProperties.Type,
            Name = await ExtractChatNameAsync(request, cancellationToken),
            PhotoUrl = "/avatar/default_avatar.png",
          },
        };

        Chat createdChat = await _chatDataStrategy.InsertOneAsync(chat, cancellationToken);
        return _mapper.Map<ChatDto>(createdChat);
      }

      private async Task<string> ExtractChatNameAsync(CreateChatRequest request, CancellationToken cancellationToken)
      {
        if (request.ChatProperties.Type != Core.Enums.ChatType.Private)
          return "Default Name";

        Guid currentUserId = _securityService.GetCurrentUserId();
        Guid userId = request.Participants.FirstOrDefault(x => x != currentUserId);
        User user = await _userDataStrategy.FindAsync(userId, cancellationToken);
        if (user is null)
          throw new EntityNotFoundException(nameof(User), userId);

        return $"{user.Name} {user.LastName}";
      }

      private async Task ValidateParticipantsAsync(CreateChatRequest request, CancellationToken cancellationToken)
      {
        foreach(Guid userId in request.Participants)
        {
          User user = await _userDataStrategy.FindAsync(userId, cancellationToken);
          if (user is null)
            throw new EntityNotFoundException(nameof(User), userId);
        }
      }

      private async Task ValidateRequestAsync(CreateChatRequest request, CancellationToken cancellationToken)
      {
        try
        {
          await Task.Yield();
          var dtoValidator = new CreateChatRequestValidator();
          await dtoValidator.ValidateRequest(request, nameof(CreateChatRequest), cancellationToken);  
        } catch (Exception ex) when (ex is not EntityValidationException)
        {
          _logger.LogCritical(ex, "Failed to validate request for {RequestName}", nameof(CreateChatRequest));
        }
      }
    }
  }
}