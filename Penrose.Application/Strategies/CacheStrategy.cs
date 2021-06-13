using Penrose.Application.Interfaces.Strategies;
using Penrose.Core.Common;
using Penrose.Core.Interfaces;
using ServiceStack.Redis;

namespace Penrose.Application.Strategies
{
    public abstract class CacheStrategy<TEntity> : ICacheDataStrategy<TEntity> where TEntity : AuditableEntity
    {

        private readonly RedisManagerPool _redisManager;
        private readonly IRedisClient _redisClient;
        
        public CacheStrategy(RedisManagerPool redisManager)
        {
            _redisManager = new RedisManagerPool("apollo");
             _redisClient = redisManager.GetClient();
        }
    }
}