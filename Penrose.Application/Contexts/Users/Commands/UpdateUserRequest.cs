using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.Extensions;
using Penrose.Application.Interfaces;
using Penrose.Application.Interfaces.UserStrategies;
using Penrose.Core.Entities;

namespace Penrose.Application.Contexts.Users.Commands
{
    public class UpdateUserRequest : IRequest<UserDto>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        
        public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest, UserDto>
        {
            private readonly IUserDataStrategy _userDataStrategy;
            private readonly ISecurityService _securityService;
            private readonly IMapper _mapper;
            
            public UpdateUserRequestHandler(
                IUserDataStrategy userDataStrategy,
                IMapper mapper, ISecurityService securityService)
            {
                _mapper = mapper;
                _securityService = securityService;
                _userDataStrategy = userDataStrategy;
            }

            public async Task<UserDto> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
            {
                Guid currentUserId = _securityService.GetCurrentUserId();
                User currentUser = await _userDataStrategy.FindAsync(currentUserId, cancellationToken);

                currentUser.Name = request.Name;
                currentUser.LastName = request.LastName;

                await _userDataStrategy.SaveAsync(currentUser);

                return _mapper.Map<UserDto>(currentUser);
            }
        }
    }
}