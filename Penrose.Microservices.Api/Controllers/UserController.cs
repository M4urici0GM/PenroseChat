using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Penrose.Application.Common;
using Penrose.Application.Contexts.Users.Commands;
using Penrose.Application.Contexts.Users.Queries;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.Extensions;
using Penrose.Core.Common;

namespace Penrose.Microservices.User.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class UserController : BasePenroseController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> FindAll([FromQuery] FindAllUsersRequest pagedRequest)
        {
            PagedResult<UserDto> users = await _mediator.Send(pagedRequest);
            return Ok(users);
        }

        [HttpGet, Route("{id:guid}")]
        public async Task<IActionResult> Find(Guid id)
        {
            UserDto user = await _mediator.Send(new FindUserRequest() {Id = id});
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateUserRequest userRequestDto)
        {
            UserDto createdUser = await _mediator.Send(userRequestDto);
            return Ok(createdUser);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
        {
            UserDto updatedUser = await _mediator.Send(request);
            return Ok(updatedUser);
        }

        [HttpGet, Route("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            Guid currentUserId = HttpContext.GetUserId();
            UserDto user = await _mediator.Send(new FindUserRequest()
            {
                Id = currentUserId
            });
            return Ok(user);
        }
    }
}