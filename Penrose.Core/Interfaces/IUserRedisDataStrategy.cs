using System;
using System.Threading.Tasks;
using Penrose.Core.Entities;

namespace Penrose.Core.Interfaces
{
    public interface IUserRedisDataStrategy : ICachingDataStrategy<User>
    {
        Task<User> GetUser(Guid userId);
        Task Save(User user);
        Task<User> Retrieve(Guid userId);
    }
}