using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Penrose.Application.Extensions;
using Penrose.Application.Strategies;
using Penrose.Core.Common;
using Penrose.Core.Entities;
using Penrose.Core.Interfaces;
using Penrose.Core.Interfaces.UserStrategies;

namespace Penrose.Application.Repositories.Users
{
    public class UserDataStrategy : DataStrategy<User>, IUserDataStragegy
    {
        private readonly DbSet<User> _userDb;
        public UserDataStrategy(IPenroseDbContext dbContext) : base(dbContext)
        {
            _userDb = dbContext.GetDbSet<User>();
        }
        
        public async Task<User> FindAsync(Guid userId, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _userDb
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<User> SaveAsync(User user)
        {
            await _userDb.AddAsync(user);
            await PenroseDbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> FindByNickname(string nickname, CancellationToken cancellationToken)
        {
            return await _userDb
                .FirstOrDefaultAsync(x => x.Nickname == nickname, cancellationToken);
        }

        public async Task<bool> NicknameExists(string nickname, CancellationToken cancellationToken)
        {
            return await _userDb
                .AnyAsync(x => x.Nickname == nickname, cancellationToken);
        }

        public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = new())
        {
            IPenroseDbContext penroseDbContext = PenroseDbContext;
            if (penroseDbContext.GetEntityEntry(user).State == EntityState.Detached)
                penroseDbContext.AttachEntity(user);

            await penroseDbContext.SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task<PagedResult<User>> FindAllAsync(PagedRequest pagedRequest, CancellationToken cancellationToken = new CancellationToken())
        {
            IQueryable<User> userQuery = _userDb
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