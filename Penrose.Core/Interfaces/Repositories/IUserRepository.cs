using System;
using System.Threading;
using System.Threading.Tasks;
using Penrose.Core.Entities;

namespace Penrose.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetFromCache(Guid userId);
        Task<User> Find(Guid userId);
        Task SaveToCache(User user);
        Task<User> SaveAsync(User user);
        Task<User> UpdateAsync(User user, CancellationToken cancellationToken = new());
    }
}