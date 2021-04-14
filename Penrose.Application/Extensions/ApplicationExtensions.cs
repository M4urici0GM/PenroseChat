using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Penrose.Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddBasicApi(this IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddApplicationRepositories();

            return services;
        }

        public static IApplicationBuilder UseBasicConfiguration(this IApplicationBuilder app,
            IHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsDevelopment())
            {
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
            });

            return app;
        }
    }
}