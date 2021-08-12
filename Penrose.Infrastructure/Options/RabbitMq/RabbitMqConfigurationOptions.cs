namespace Penrose.Infrastructure.Options.RabbitMq
{
    public class RabbitMqConfigurationOptions
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public string VirtualHost { get; set; }
    }
}