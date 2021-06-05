using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Penrose.Application.Common;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.Interfaces.UserStrategies;
using Penrose.Core.Common;
using Penrose.Core.Entities;

namespace Penrose.Application.Contexts.Users.Queries
{
    public class FindAllUsersRequest : PagedRequest, IRequest<PagedResult<UserDto>>
    {
        public class FindAllUsersHandler : IRequestHandler<FindAllUsersRequest, PagedResult<UserDto>>
        {
            private readonly IUserDataStrategy _userDataStrategy;
            private readonly IMapper _mapper;
            
            public FindAllUsersHandler(
                IUserDataStrategy userDataStrategy,
                IMapper mapper)
            {
                _userDataStrategy = userDataStrategy;
                _mapper = mapper;
            }

            public async Task<PagedResult<UserDto>> Handle(FindAllUsersRequest request, CancellationToken cancellationToken)
            {
                PagedResult<User> users = await _userDataStrategy.FindAllAsync(request, CancellationToken.None);
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