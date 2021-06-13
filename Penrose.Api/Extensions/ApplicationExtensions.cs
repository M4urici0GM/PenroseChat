using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Penrose.Application.Extensions;

namespace Penrose.Microservices.User.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddBasicApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationJwtBearer(configuration);
            services
                .AddControllers(options =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                        .Build();

                    options.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });

            services.AddHttpContextAccessor();
            services.ConfigureAutoMapper();
            services.AddApplicationDataStrategies();
            services.AddApplicationServices();
            services.AddApplicationOptions(configuration);
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}