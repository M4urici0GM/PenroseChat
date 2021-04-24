using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.DataTransferObjects.Requests;
using Penrose.Application.Interfaces;
using Penrose.Core.Common;

namespace Penrose.Microservices.User.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> FindAll([FromQuery] PagedRequest pagedRequest)
        {
            PagedResult<UserDto> users = await _userService.FindAllAsync(pagedRequest);
            return Ok(users);
        }

        [HttpGet, Route("{id:guid}")]
        public async Task<IActionResult> Find(Guid id)
        {
            UserDto userDto = await _userService.FindById(id);
            return Ok(new
            {
                user = userDto,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto userDto)
        {
            UserDto createdUser = await _userService.Create(userDto);
            return Ok(new
            {
                user = createdUser,
            });
        }
    }
}