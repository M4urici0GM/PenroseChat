using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Penrose.Core.Entities;
using Penrose.Core.Interfaces;
using Penrose.Core.Interfaces.Repositories;

namespace Penrose.Application.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly IMssqlDataStrategy<User> _mssqlDataStrategy;
        private readonly ICachingDataStrategy<User> _redisDataStrategy;

        public UserRepository(IMssqlDataStrategy<User> mssqlDataStrategy, ICachingDataStrategy<User> redisDataStrategy)
        {
            _mssqlDataStrategy = mssqlDataStrategy;
            _redisDataStrategy = redisDataStrategy;
        }

        public Task<User> GetFromCache(Guid userId)
        {
            return _redisDataStrategy.Get(userId);
        }

        public Task SaveToCache(User user)
        {
            return _redisDataStrategy.Set(user);
        }

        public Task<User> Find(Guid userId)
        {
            return _mssqlDataStrategy.GetDbSet().FindAsync(userId).AsTask();
        }

        public async Task<User> SaveAsync(User user)
        {
            await _mssqlDataStrategy.GetDbSet().AddAsync(user);
            await _mssqlDataStrategy.GetContext().SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = new())
        {
            var penroseDbContext = _mssqlDataStrategy.GetContext();
            if (penroseDbContext.GetEntityEntry(user).State == EntityState.Detached)
                penroseDbContext.AttachEntity(user);

            await penroseDbContext.SaveChangesAsync(cancellationToken);

            return user;
        }
    }
}