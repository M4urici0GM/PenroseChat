using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Penrose.Core.Interfaces;
using StackExchange.Redis;

namespace Penrose.Persistence.Context
{
    public class RedisClient : IRedisClient
    {
        private readonly IDatabase _redisDatabase;

        public RedisClient(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(nameof(RedisClient));
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidCastException("Missing redis connection string!");

            var redisDatabase = ConnectionMultiplexer.ConnectAsync(connectionString)
                .GetAwaiter()
                .GetResult();

            _redisDatabase = redisDatabase.GetDatabase();
        }

        public Task<RedisValue> GetString(Guid id)
        {
            return _redisDatabase.StringGetAsync(id.ToString());
        }

        public Task<bool> SetString(string key, string value, TimeSpan ttl)
        {
            return _redisDatabase.StringSetAsync(key, value, ttl);
        }

        public Task<bool> SetString(Guid key, string value, TimeSpan ttl)
        {
            return SetString(key.ToString(), value, ttl);
        }
    }
}