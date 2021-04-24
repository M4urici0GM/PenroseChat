using Microsoft.Extensions.DependencyInjection;
using Penrose.Application.Interfaces;
using Penrose.Application.Services;

namespace Penrose.Application.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IHashingService, HashingService>();
        }
    }
}