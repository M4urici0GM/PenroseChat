using System;
using System.Threading;
using System.Threading.Tasks;
using Penrose.Application.DataTransferObjects;
using Penrose.Application.DataTransferObjects.Requests;
using Penrose.Core.Common;

namespace Penrose.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> Create(CreateUserDto userDto);
        Task<UserDto> FindById(Guid id);

        Task<PagedResult<UserDto>> FindAllAsync(PagedRequest pagedRequest,
            CancellationToken cancellationToken = new CancellationToken());
    }
}