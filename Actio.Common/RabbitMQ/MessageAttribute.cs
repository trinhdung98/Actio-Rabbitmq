using System;

namespace Actio.Common.RabbitMQ
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MessageAttribute : Attribute
    {
        public string Exchange { get; }
        public string RoutingKey { get; }
        public string Queue { get; }

        public MessageAttribute(string exchange, string routingKey, string queue)
        {
            Exchange = exchange;
            RoutingKey = routingKey;
            Queue = queue;
        }
    }
}