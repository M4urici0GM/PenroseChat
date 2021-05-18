using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.Extensions;
using Penrose.Core.Entities;
using Penrose.Core.Interfaces.UserStrategies;

namespace Penrose.Application.Contexts.Users.Commands
{
    public class UpdateUserRequest : IRequest<UserDto>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        
        public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest, UserDto>
        {
            private readonly IUserDataStrategy _userDataStrategy;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _contextAccessor;
            
            public UpdateUserRequestHandler(
                IUserDataStrategy userDataStrategy,
                IMapper mapper,
                IHttpContextAccessor httpContextAccessor)
            {
                _mapper = mapper;
                _userDataStrategy = userDataStrategy;
                _contextAccessor = httpContextAccessor;
            }

            public async Task<UserDto> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
            {
                Guid currentUserId = _contextAccessor.GetUserId();
                User currentUser = await _userDataStrategy.FindAsync(currentUserId, cancellationToken);

                currentUser.Name = request.Name;
                currentUser.LastName = request.LastName;

                await _userDataStrategy.SaveAsync(currentUser);

                return _mapper.Map<UserDto>(currentUser);
            }
        }
    }
}