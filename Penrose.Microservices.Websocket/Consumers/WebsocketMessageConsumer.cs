using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Penrose.Core.Interfaces.Clients;
using Penrose.Microservices.Websocket.Contexts.Messages;
using Penrose.Microservices.Websocket.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Penrose.Microservices.Websocket.Consumers
{
    public class WebsocketMessageConsumer : BackgroundService
    {
        private readonly IModel _mqChannel;
        private readonly WebsocketSubscriberOptions _subscriberOptions;
        private readonly IMediator _mediator;
        private readonly string _queueName;

        public WebsocketMessageConsumer(
            IMediator mediator,
            IRabbitMqClient rabbitMqClient,
            IOptions<WebsocketSubscriberOptions> subscriberOptions)
        {
            IConnection mqConnection = rabbitMqClient.GetConnection();
            
            _mediator = mediator;
            _subscriberOptions = subscriberOptions.Value;
            _mqChannel = mqConnection.CreateModel();
            _queueName = _mqChannel.QueueDeclare().QueueName;
        }

        private void StartQueue()
        {
            string exchangeName = _subscriberOptions.MessageExchangeName;

            _mqChannel.ExchangeDeclare(_subscriberOptions.MessageExchangeName, ExchangeType.Fanout);
            _mqChannel.QueueBind(_queueName, exchangeName, "");
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            StartQueue();
            CreateMessageConsumer(cancellationToken);

            return Task.CompletedTask;
        }

        private void CreateMessageConsumer(CancellationToken cancellationToken)
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(_mqChannel);
            consumer.Received += async (object _, BasicDeliverEventArgs e) =>
            {
                await OnMessageReceived(e, cancellationToken);
            };

            _mqChannel.BasicConsume(_queueName, true, consumer);
        }

        private async Task OnMessageReceived(BasicDeliverEventArgs e, CancellationToken cancellationToken)
        {
            DateTime utcNow = DateTime.UtcNow;
            byte[] messageBuffer = e.Body.ToArray();

            await _mediator.Send(new MessageReceivedRequest
            {
                MessageBuffer = messageBuffer,
                ReceivedAt = utcNow,
            }, cancellationToken);
        }
    }
}