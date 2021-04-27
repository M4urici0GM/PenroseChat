﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Penrose.Application.Contexts.Queries;
using Penrose.Application.DataTransferObjects;
using Penrose.Core.Entities;
using Penrose.Core.Exceptions;
using Penrose.Core.Interfaces.Repositories;

namespace Penrose.Application.Contexts.Commands
{
    public class UpdateUserRequest : IRequest<UserDto>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        
        public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest, UserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;
            
            public UpdateUserRequestHandler(
                IUserRepository userRepository,
                IMapper mapper,
                IMediator mediator)
            {
                _mapper = mapper;
                _userRepository = userRepository;
                _mediator = mediator;
            }

            public async Task<UserDto> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
            {
                Guid currentUserId = Guid.Empty;
                User currentUser = await _mediator.Send(new FindUserRequest() { Id = currentUserId });

                currentUser.Name = request.Name;
                currentUser.LastName = request.LastName;

                await _userRepository.SaveAsync(currentUser);

                return _mapper.Map<UserDto>(currentUser);
            }
        }
    }
}