using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Penrose.Core.Interfaces
{
    public interface IRedisClient
    {
        Task<RedisValue> GetString(Guid id);
        Task<bool> SetString(string key, string value, TimeSpan ttl);
        Task<bool> SetString(Guid key, string value, TimeSpan ttl);
    }
}