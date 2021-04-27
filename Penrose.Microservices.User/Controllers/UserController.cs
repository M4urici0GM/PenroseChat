using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Penrose.Application.Common;
using Penrose.Application.Contexts.Commands;
using Penrose.Application.Contexts.Queries;
using Penrose.Application.DataTransferObjects;
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
            Core.Entities.User user = await _mediator.Send(new FindUserRequest() {Id = id});
            
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateUserRequest userRequestDto)
        {
            UserDto createdUser = await _mediator.Send(userRequestDto);
            return Ok(createdUser);
        }
    }
}