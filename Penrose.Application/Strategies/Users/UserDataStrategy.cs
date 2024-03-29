﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Penrose.Application.Common;
using Penrose.Application.Extensions;
using Penrose.Core.Common;
using Penrose.Core.Entities;
using Penrose.Core.Interfaces;
using Penrose.Application.Interfaces.UserStrategies;

namespace Penrose.Application.Strategies.Users
{
    public class UserDataStrategy : DataStrategy<User>, IUserDataStrategy
    {
        private readonly DbSet<User> _userDb;
        public UserDataStrategy(IPenroseDbContext dbContext) : base(dbContext)
        {
            _userDb = dbContext.GetDbSet<User>();
        }
        
        public async Task<User> FindAsync(Guid userId, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _userDb
                .Include(x => x.Properties)
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<User> SaveAsync(User user)
        {
            await _userDb.AddAsync(user);
            await PenroseDbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> FindByNicknameAsync(string nickname, CancellationToken cancellationToken)
        {
            return await _userDb
                .Include(x => x.Properties)
                .FirstOrDefaultAsync(x => x.Nickname == nickname, cancellationToken);
        }

        public async Task<bool> NicknameExistsAsync(string nickname, CancellationToken cancellationToken)
        {
            return await _userDb
                .Include(x => x.Properties)
                .AnyAsync(x => x.Nickname == nickname, cancellationToken);
        }

        public async Task<bool> NicknameOrEmailExists(string nickname, string email, CancellationToken cancellationToken)
        {
            return await _userDb
                .Include(x => x.Properties)
                .AnyAsync(x => x.Nickname == nickname || x.Email == email, cancellationToken);
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
                .Include(x => x.Properties)
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
                PageSize = pagedRequest.PageSize,
                Records = users,
            };
        }
    }
}