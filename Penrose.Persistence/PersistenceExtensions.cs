using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Penrose.Core.Interfaces;
using Penrose.Persistence.Context;
using Penrose.Persistence.Strategies;

namespace Penrose.Persistence
{
    public static class PersistenceExtensions
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            IRedisClient redisClient = new RedisClient(configuration);
            var connectionString = configuration.GetConnectionString(nameof(PenroseDbContext));

            services
                .AddDbContext<IPenroseDbContext, PenroseDbContext>(options =>
                    options.UseSqlServer(connectionString));

            services.AddSingleton(redisClient);
            services.AddScoped(typeof(ICachingDataStrategy<>), typeof(CachingDataStrategy<>));
            services.AddScoped(typeof(IMssqlDataStrategy<>), typeof(MssqlDataStrategy<>));
        }
    }
}