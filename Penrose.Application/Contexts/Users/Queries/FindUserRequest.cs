using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.Interfaces.UserStrategies;
using Penrose.Core.Entities;
using Penrose.Core.Exceptions;


namespace Penrose.Application.Contexts.Users.Queries
{
    public class FindUserRequest : IRequest<UserDto>
    {
        public Guid Id { get; set; }

        public class FindUserRequestHandler : IRequestHandler<FindUserRequest, UserDto>
        {
            private readonly IUserDataStrategy _userDataStrategy;
            private readonly IMapper _mapper;
             
            public FindUserRequestHandler(
                IUserDataStrategy userDataStrategy,
                IMapper mapper)
            {
                _userDataStrategy = userDataStrategy;
                _mapper = mapper;
            }
            
            public async Task<UserDto> Handle(FindUserRequest request, CancellationToken cancellationToken)
            {
                User user = await _userDataStrategy.FindAsync(request.Id, cancellationToken);
                if (user == null)
                    throw new EntityNotFoundException(nameof(User), request.Id);

                return _mapper.Map<UserDto>(user);
            }
        }
    }
}