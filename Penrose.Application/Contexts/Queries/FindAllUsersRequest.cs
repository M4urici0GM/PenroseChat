using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Penrose.Application.DataTransferObjects;
using Penrose.Core.Common;
using Penrose.Core.Entities;
using Penrose.Core.Interfaces.Repositories;

namespace Penrose.Application.Contexts.Queries
{
    public class FindAllUsersRequest : PagedRequest, IRequest<PagedResult<UserDto>>
    {
        public class FindAllUsersHandler : IRequestHandler<FindAllUsersRequest, PagedResult<UserDto>>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            
            public FindAllUsersHandler(
                IUserRepository userRepository,
                IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public async Task<PagedResult<UserDto>> Handle(FindAllUsersRequest request, CancellationToken cancellationToken)
            {
                PagedResult<User> users = await _userRepository.FindAllAsync(request, CancellationToken.None);
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