using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Penrose.Application.DataTransferObjects;
using Penrose.Core.Common;
using Penrose.Core.Entities;
using Penrose.Core.Interfaces.UserStrategies;

namespace Penrose.Application.Contexts.Users.Queries
{
    public class FindAllUsersRequest : PagedRequest, IRequest<PagedResult<UserDto>>
    {
        public class FindAllUsersHandler : IRequestHandler<FindAllUsersRequest, PagedResult<UserDto>>
        {
            private readonly IUserDataStragegy _userDataStragegy;
            private readonly IMapper _mapper;
            
            public FindAllUsersHandler(
                IUserDataStragegy userDataStragegy,
                IMapper mapper)
            {
                _userDataStragegy = userDataStragegy;
                _mapper = mapper;
            }

            public async Task<PagedResult<UserDto>> Handle(FindAllUsersRequest request, CancellationToken cancellationToken)
            {
                PagedResult<User> users = await _userDataStragegy.FindAllAsync(request, CancellationToken.None);
                return new PagedResult<UserDto>()
                {
                    Count = users.Count,
                    Offset = users.Offset,
                    Pagesize = users.Pagesize,
                    Records = _mapper.Map<IEnumerable<UserDto>>(users.Records),
                };
            }
        }
    }
}