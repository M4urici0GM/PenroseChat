using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Penrose.Application.Interfaces;
using Penrose.Application.Services;

namespace Penrose.Application.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient<IHashingService, HashingService>();
            services.AddTransient<IJwtService, JwtService>();
        }
    }
}