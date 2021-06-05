using Microsoft.Extensions.DependencyInjection;
using Penrose.Application.Interfaces.ChatStrategies;
using Penrose.Application.Strategies.Chats;
using Penrose.Application.Strategies.Users;
using Penrose.Application.Interfaces.UserStrategies;

namespace Penrose.Application.Extensions
{
    public static class DataStregyExtensions
    {
        public static void AddApplicationDataStrategies(this IServiceCollection services)
        {
            services.AddTransient<IUserDataStrategy, UserDataStrategy>();
            services.AddTransient<IChatDataStrategy, ChatDataStrategy>();
        }
    }
}