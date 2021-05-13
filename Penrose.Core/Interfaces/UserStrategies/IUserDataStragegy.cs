using System;
using System.Threading;
using System.Threading.Tasks;
using Penrose.Core.Common;
using Penrose.Core.Entities;

namespace Penrose.Core.Interfaces.UserStrategies
{
    public interface IUserDataStragegy
    {
        Task<User> FindAsync(Guid userId, CancellationToken cancellationToken = new CancellationToken());
        Task<User> SaveAsync(User user);
        Task<User> FindByNickname(string nickname, CancellationToken cancellationToken);
        Task<bool> NicknameExists(string nickname, CancellationToken cancellationToken);
        Task<PagedResult<User>> FindAllAsync(PagedRequest pagedRequest, CancellationToken cancellationToken);
    }
}