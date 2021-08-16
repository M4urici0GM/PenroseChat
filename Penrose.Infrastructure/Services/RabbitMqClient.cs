using Microsoft.Extensions.Options;
using Penrose.Core.Interfaces.Clients;
using Penrose.Infrastructure.Options.RabbitMq;
using RabbitMQ.Client;

namespace Penrose.Infrastructure.Services
{
    public class RabbitMqClient : IRabbitMqClient
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _mqConnection;

        public RabbitMqClient(IOptions<RabbitMqConfigurationOptions> rabbitMqConfigurationOptions)
        {
            RabbitMqConfigurationOptions mqConfiguration = rabbitMqConfigurationOptions.Value;
            
            _connectionFactory = new ConnectionFactory()
            {
                UserName = mqConfiguration.Username,
                Password = mqConfiguration.Password,
                VirtualHost = mqConfiguration.VirtualHost,
                HostName = mqConfiguration.HostName,
                DispatchConsumersAsync = true,
            };
        }

        private IConnection CreateConnection()
        {
            _mqConnection = _connectionFactory.CreateConnection();
            return _mqConnection;
        }
        
        public IConnection GetConnection()
        {
            return _mqConnection ?? CreateConnection();
        }
    }
}