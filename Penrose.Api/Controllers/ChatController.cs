using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Penrose.Application.Common;
using Penrose.Application.Contexts.Chats.Commands;
using Penrose.Application.Contexts.Chats.Queries;
using Penrose.Application.Contexts.Messages.Queries;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.Interfaces;

namespace Penrose.Microservices.User.Controllers
{
  [ApiController, Route("api/[controller]")]
  public class ChatController : BasePenroseController
  {
    private readonly IMediator _mediator;
    
    public ChatController(IMediator mediator, ISecurityService securityService) : base(securityService)
    {
      _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserChats([FromQuery] GetUserChatsRequest request)
    {
      PagedResult<ChatDto> userChats = await _mediator.Send(request);
      return Ok(userChats);
    }

    [HttpGet, Route("{chatId:guid}/messages")]
    public async Task<IActionResult> GetChatMessages(Guid chatId, [FromQuery] FindChatMessagesRequest request)
    {
      PagedResult<ChatMessageDto> requestResult = await _mediator.Send(request);
      return Ok(requestResult);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewChat([FromBody] CreateChatRequest request, CancellationToken cancellationToken)
    {
      ChatDto createdChat = await _mediator.Send(request, cancellationToken);
      return Ok(createdChat);
    }
  }
}