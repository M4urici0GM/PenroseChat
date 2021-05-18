using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Penrose.Application.Common;
using Penrose.Application.Contexts.Users.Commands;
using Penrose.Application.DataTransferObjects;

namespace Penrose.Microservices.User.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class AuthenticateController : BasePenroseController
    {
        private readonly IMediator _mediator;
        
        public AuthenticateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
        {
            AuthenticatedUserDto userDto = await _mediator.Send(request);
            return Created(userDto);
        }
    }
}