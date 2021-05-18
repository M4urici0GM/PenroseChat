using Microsoft.Extensions.DependencyInjection;
using Penrose.Application.Repositories.Users;
using Penrose.Core.Interfaces.UserStrategies;

namespace Penrose.Application.Extensions
{
    public static class DataStregyExtensions
    {
        public static void AddApplicationDataStrategies(this IServiceCollection services)
        {
            services.AddScoped<IUserDataStrategy, UserDataStrategy>();
        }
    }
}