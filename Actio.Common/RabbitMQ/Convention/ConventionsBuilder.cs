using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;

namespace Actio.Common.RabbitMQ.Convention
{
    public class ConventionsBuilder : IConventionsBuilder
    {
        private readonly RabbitMqOptions _options;
        public ConventionsBuilder(IOptions<RabbitMqOptions> options)
        {
            _options = options.Value;
        }

        public string GetExchange(Type type)
        {
            var attribute = GetAttribute(type);
            var exchange = string.IsNullOrWhiteSpace(attribute?.Exchange)
                ? string.IsNullOrWhiteSpace(_options.Exchange?.Name) ? type.Assembly.GetName().Name :
                _options.Exchange.Name
                : attribute.Exchange;

            return WithCasing(exchange);
        }

        public string GetQueue(Type type)
        {
            var attribute = GetAttribute(type);
            var exchange = string.IsNullOrWhiteSpace(attribute?.Exchange) ? _options.Exchange.Name : attribute.Exchange;
            exchange = string.IsNullOrWhiteSpace(exchange) ? string.Empty : $"{exchange}.";
            var queue = string.IsNullOrWhiteSpace(attribute?.Queue)
                ? $"{type.Assembly.GetName().Name}/{exchange}{type.Name}"
                : attribute.Queue;

            return WithCasing(queue);
        }

        public string GetRoutingKey(Type type)
        {
            var attribute = GetAttribute(type);
            var routingKey = string.IsNullOrWhiteSpace(attribute?.RoutingKey) ? type.Name : attribute.RoutingKey;

            return WithCasing(routingKey);
        }

        private string WithCasing(string value) => SnakeCase(value);

        private static string SnakeCase(string value)
            => string.Concat(value.Select((x, i) =>
                    i > 0 && value[i - 1] != '.' && value[i - 1] != '/' && char.IsUpper(x) ? "_" + x : x.ToString()))
                .ToLowerInvariant();

        private static MessageAttribute GetAttribute(MemberInfo type) => type.GetCustomAttribute<MessageAttribute>();
    }
}