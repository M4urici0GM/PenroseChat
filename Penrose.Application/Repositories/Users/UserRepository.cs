using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Penrose.Application.Extensions;
using Penrose.Application.Interfaces;
using Penrose.Core.Common;
using Penrose.Core.Entities;
using Penrose.Core.Interfaces;
using Penrose.Core.Interfaces.Repositories;

namespace Penrose.Application.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataStrategy<User> _dataStrategy;

        public UserRepository(IDataStrategy<User> dataStrategy)
        {
            _dataStrategy = dataStrategy;
        }
        
        public Task<User> Find(Guid userId)
        {
            return _dataStrategy.GetDbSet()
                .FindAsync(userId)
                .AsTask();
        }

        public async Task<User> SaveAsync(User user)
        {
            await _dataStrategy.GetDbSet().AddAsync(user);
            await _dataStrategy.GetContext().SaveChangesAsync();

            return user;
        }

        public Task<User> FindByNickname(string nickname)
        {
            return _dataStrategy.GetDbSet()
                .FirstOrDefaultAsync(x => x.Nickname == nickname);
        }

        public Task<bool> NicknameExists(string nickname)
        {
            return _dataStrategy.GetDbSet()
                .AnyAsync(x => x.Nickname == nickname);
        }

        public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = new())
        {
            var penroseDbContext = _dataStrategy.GetContext();
            if (penroseDbContext.GetEntityEntry(user).State == EntityState.Detached)
                penroseDbContext.AttachEntity(user);

            await penroseDbContext.SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task<PagedResult<User>> FindAllAsync(PagedRequest pagedRequest, CancellationToken cancellationToken = new CancellationToken())
        {
            IQueryable<User> userQuery = _dataStrategy
                .GetDbSet()
                .AsQueryable()
                .Where(x => x.IsActive);

            if (!string.IsNullOrEmpty(pagedRequest.OrderBy))
            {
                if (pagedRequest.OrderBy.EndsWith("name"))
                    userQuery = userQuery.ApplyOrdering(x => x.Name, pagedRequest.OrderBy);

                if (pagedRequest.OrderBy.EndsWith("email"))
                    userQuery = userQuery.ApplyOrdering(x => x.Email, pagedRequest.OrderBy);
            }

            int recordCount = await userQuery.CountAsync(cancellationToken);
            IEnumerable<User> users = await userQuery
                .ApplyPagination(pagedRequest)
                .ToListAsync(cancellationToken);

            return new PagedResult<User>()
            {
                Count = recordCount,
                Offset = pagedRequest.Offset,
                Pagesize = pagedRequest.Pagesize,
                Records = users,
            };
        }
    }
}