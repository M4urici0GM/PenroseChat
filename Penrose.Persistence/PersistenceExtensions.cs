using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Penrose.Core.Interfaces;
using Penrose.Persistence.Context;

namespace Penrose.Persistence
{
    public static class PersistenceExtensions
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Default");

            services
                .AddDbContext<IPenroseDbContext, PenroseDbContext>(options =>
                    options.UseSqlServer(connectionString));
        }
    }
}