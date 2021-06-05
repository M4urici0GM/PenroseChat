using ServiceStack.Redis;
using Penrose.Core.Entities;
using Penrose.Application.Interfaces.UserStrategies;

namespace Penrose.Application.Strategies.Users
{
    public class UserCacheStrategy : CacheStrategy<User>, IUserCacheStrategy
    {
        public UserCacheStrategy(RedisManagerPool redisManager) : base(redisManager)
        {}
    }
}