using System;

namespace Penrose.Infrastructure.EventBus.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BindToExchangeAttribute : Attribute
    {
        public string ExchangeName { get; }

        public BindToExchangeAttribute(string exchangeName)
        {
            ExchangeName = exchangeName;
        }
    }
}