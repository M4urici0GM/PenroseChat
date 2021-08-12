using RabbitMQ.Client;

namespace Penrose.Core.Interfaces.Clients
{
    public interface IRabbitMqClient
    {
        IConnection GetConnection();
    }
}