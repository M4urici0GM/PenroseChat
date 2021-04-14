using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Penrose.Core.Common;
using Penrose.Core.Exceptions;
using Penrose.Core.Interfaces;

namespace Penrose.Persistence.Strategies
{
    public class CachingDataStrategy<TEntity> : ICachingDataStrategy<TEntity> where TEntity : AuditableEntity
    {
        private readonly IRedisClient _redisClient;

        public CachingDataStrategy(IRedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        public async Task<bool> Set(TEntity entity)
        {
            var entityStr = entity.ToJson();
            var hasSet = await _redisClient.SetString(entity.Id, entityStr, TimeSpan.FromMinutes(30));
            if (!hasSet)
                throw new RedisEntityException(nameof(TEntity), entity.Id.ToString(),
                    "Failed to save entity to the database");

            return true;
        }

        public async Task<TEntity> Get(Guid entityId)
        {
            var redisValue = await _redisClient.GetString(entityId);
            if (!redisValue.HasValue)
                throw new RedisEntityException(nameof(TEntity), entityId.ToString(), "Entity not found on database.");

            return JsonConvert.DeserializeObject<TEntity>(redisValue);
        }
    }
}