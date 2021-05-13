using Penrose.Application.Strategies;
using Penrose.Core.Entities;
using Penrose.Core.Interfaces.UserStrategies;
using ServiceStack.Redis;

namespace Penrose.Application.Repositories.Users
{
    public class UserCacheStrategy : CacheStrategy<User>, IUserCacheStrategy
    {
        public UserCacheStrategy(RedisManagerPool redisManager) : base(redisManager)
        {}
    }
}