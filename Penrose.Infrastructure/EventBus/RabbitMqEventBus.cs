using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Penrose.Core.Interfaces.Clients;
using Penrose.Infrastructure.EventBus.Attributes;
using Penrose.Infrastructure.EventBus.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Penrose.Infrastructure.EventBus
{
    public class RabbitMqEventBus : IRabbitMqEventBus
    {
        private readonly IRabbitMqClient _connectionClient;
        private readonly IMediator _mediator;

        private readonly ConcurrentDictionary<Guid, Type> _eventHandlers;
        private readonly ConcurrentDictionary<string, IEnumerable<Guid>> _availableEventHandlers;

        public RabbitMqEventBus(IRabbitMqClient rabbitMqClient, IMediator mediator)
        {
            _availableEventHandlers = new ConcurrentDictionary<string, IEnumerable<Guid>>();
            _eventHandlers = new ConcurrentDictionary<Guid, Type>();
            _connectionClient = rabbitMqClient;
            _mediator = mediator;
        }

        private BindToExchangeAttribute GetExchangeAttribute(Type type)
        {
            BindToExchangeAttribute classAttribute = type
                .GetCustomAttributes(typeof(BindToExchangeAttribute), true)
                .FirstOrDefault() as BindToExchangeAttribute;

            if (classAttribute == null)
                throw new Exception("Missing Exchange attribute");

            return classAttribute;
        }
        public void Publish<T>(T @event) where T : IEvent
        {
            Type tType = @event.GetType();
            BindToExchangeAttribute exchangeAttribute = GetExchangeAttribute(tType);
            string exchangeName = exchangeAttribute.ExchangeName;
            
            
            using IConnection connection = _connectionClient.GetConnection();
            using IModel channel = connection.CreateModel();
            
            channel.BasicPublish(
                exchangeName, 
                "",
                false,
                null,
                new ReadOnlyMemory<byte>(@event.Buffer));
        }

        public void Subscribe<T>() where T : IEventConsumer
        {
            using IConnection connection = _connectionClient.GetConnection();
            using IModel channel = connection.CreateModel();

            Type eventHandler = typeof(T);
            BindToExchangeAttribute classAttribute = GetExchangeAttribute(eventHandler);
            string exchangeName = classAttribute.ExchangeName;
            string queueName = channel.QueueDeclare().QueueName;

            channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);
            channel.QueueBind(queueName, exchangeName, "");
            AddOrUpdateHandlerToEventDictionary(exchangeName, eventHandler);
            
            AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += ConsumerOnReceivedAsync;

            channel.BasicConsume(queueName, false, consumer);
        }
        
        private async Task ConsumerOnReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            string exchangeName = e.Exchange;
            byte[] payload = e.Body.ToArray();
            List<Guid> eventHandlersForExchange = GetEventHandlerTypes(exchangeName).ToList();
            
            if (!eventHandlersForExchange.Any())
                throw new Exception($"There's no event handler for exchange {exchangeName}.");

            foreach (Guid exchangeId in eventHandlersForExchange)
            {
                Type handlerType = GetHandlerType(exchangeId);
                IEventConsumer eventConsumer = CreateEventHandlerInstance(handlerType);

                await eventConsumer.HandleEventAsync(exchangeName, payload);
            }
        }

        private IEventConsumer CreateEventHandlerInstance(Type handlerType)
        {
            IEventConsumer eventConsumer = Activator.CreateInstance(handlerType, _mediator) as IEventConsumer;
            if (eventConsumer == null)
                throw new Exception($"Failed to create instance of {handlerType.Name}.");

            return eventConsumer;
        }

        private Type GetHandlerType(Guid exchangeId)
        {
            _eventHandlers.TryGetValue(exchangeId, out Type handlerType);
            if (handlerType == null)
                throw new Exception($"EventHandler Id {exchangeId} not found.");

            return handlerType;
        }

        private Guid AddEventTypeToDictionary(Type eventHandler)
        {
            Guid currentId = Guid.NewGuid();
            bool hasInserted = _eventHandlers.TryAdd(currentId, eventHandler);
            if (!hasInserted)
                throw new Exception("Failed to insert event handler to list.");

            return currentId;
        }
        
        private void AddOrUpdateHandlerToEventDictionary(string exchangeName, Type eventHandler)
        {
            List<Guid> eventHandlers = GetEventHandlerTypes(exchangeName).ToList();
            Guid eventHandlerId = AddEventTypeToDictionary(eventHandler);
            
            eventHandlers.Add(eventHandlerId);
            _availableEventHandlers[exchangeName] = eventHandlers;
        }

        private IEnumerable<Guid> GetEventHandlerTypes(string exchangeName)
        {
            _availableEventHandlers.TryGetValue(exchangeName, out IEnumerable<Guid> eventHandlerIds);
            return eventHandlerIds ?? new List<Guid>();
        }
    }
}