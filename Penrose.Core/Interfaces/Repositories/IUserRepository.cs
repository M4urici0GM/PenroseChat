using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Penrose.Core.Common;
using Penrose.Core.Entities;

namespace Penrose.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> FindAsync(Guid userId, CancellationToken cancellationToken = new CancellationToken());
        Task<User> SaveAsync(User user);
        Task<User> FindByNickname(string nickname);
        Task<bool> NicknameExists(string nickname);
        Task<PagedResult<User>> FindAllAsync(PagedRequest pagedRequest, CancellationToken cancellationToken);
    }
}