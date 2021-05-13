using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Penrose.Application.DataTransferObjects;
using Penrose.Core.Entities;
using Penrose.Core.Exceptions;
using Penrose.Core.Interfaces.UserStrategies;

namespace Penrose.Application.Contexts.Queries
{
    public class FindUserRequest : IRequest<User>
    {
        public Guid Id { get; set; }

        public class FindUserRequestHandler : IRequestHandler<FindUserRequest, User>
        {
            private readonly IMapper _mapper;
            private readonly IUserDataStragegy _userDataStragegy;
            
            public FindUserRequestHandler(IUserDataStragegy userDataStragegy)
            {
                _userDataStragegy = userDataStragegy;
            }
            
            public async Task<User> Handle(FindUserRequest request, CancellationToken cancellationToken)
            {
                User user = await _userDataStragegy.FindAsync(request.Id, cancellationToken);
                if (user == null)
                    throw new EntityNotFoundException(nameof(User), request.Id);

                return user;
            }
        }
    }
}