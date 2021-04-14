using Microsoft.Extensions.DependencyInjection;
using Penrose.Application.Repositories.Users;
using Penrose.Core.Interfaces.Repositories;

namespace Penrose.Application.Extensions
{
    public static class RepositoryExtensions
    {
        public static void AddApplicationRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}