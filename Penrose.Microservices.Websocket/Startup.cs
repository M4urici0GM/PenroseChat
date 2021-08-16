using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Penrose.Core.Interfaces.Clients;
using Penrose.Infrastructure.EventBus;
using Penrose.Infrastructure.EventBus.Interfaces;
using Penrose.Infrastructure.Options.RabbitMq;
using Penrose.Infrastructure.Services;
using Penrose.Microservices.Websocket.Consumers;
using Penrose.Microservices.Websocket.Middlewares;
using Penrose.Microservices.Websocket.Options;
using Penrose.Microservices.Websocket.Services;

namespace Penrose.Microservices.Websocket
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<WebsocketSubscriberOptions>(
                _configuration.GetSection(nameof(WebsocketSubscriberOptions)));
            services.Configure<RabbitMqConfigurationOptions>(
                _configuration.GetSection(nameof(RabbitMqConfigurationOptions)));


            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSingleton<IRabbitMqClient, RabbitMqClient>();
            services.AddSingleton<IWebsocketManager, WebsocketManager>();
            services.AddSingleton<IRabbitMqEventBus>(serviceProvider =>
            {
                IMediator mediator = serviceProvider.GetRequiredService<IMediator>();
                IRabbitMqClient rabbitMqClient = serviceProvider.GetRequiredService<IRabbitMqClient>();
                RabbitMqEventBus eventBus = new RabbitMqEventBus(rabbitMqClient, mediator);
                
                eventBus.Subscribe<WebsocketNotificationConsumer>();

                return eventBus;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseMiddleware<WebsocketMiddleware>();
        }
    }
}