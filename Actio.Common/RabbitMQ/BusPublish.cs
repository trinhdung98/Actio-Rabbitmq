using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Actio.Common.Messages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Actio.Common.RabbitMQ
{
    public class BusPublish : IBusPublish
    {
        private readonly RabbitMqOptions _options;
        private readonly IConventionsBuilder _conventionsBuilder;
        private readonly IConnection _connection;
        public BusPublish(
            IOptions<RabbitMqOptions> options, 
            IConventionsBuilder conventionsBuilder,
            IConnection connection)
        {
            _options = options.Value;
            _conventionsBuilder = conventionsBuilder;
            _connection = connection;
        }
        public Task PublishAsync<T>(T message) where T : class
        {
            
            using (var channel = _connection.CreateModel())
            {
                var exchanges = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.IsDefined(typeof(MessageAttribute), false))
                    .Select(t => t.GetCustomAttribute<MessageAttribute>().Exchange)
                    .Distinct()
                    .ToList();

                if (_options.Exchange.Declare)
                {
                    channel.ExchangeDeclare(
                        exchange: _options.Exchange.Name,
                        type: _options.Exchange.Type,
                        durable: _options.Exchange.Durable,
                        autoDelete: _options.Exchange.AutoDelete
                    );
                }
                
                foreach (var exchange in exchanges)
                {
                    if (exchange.Equals(_options.Exchange?.Name, 
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }
                    channel.ExchangeDeclare(exchange, "topic", true);
                }
                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(
                    exchange: _conventionsBuilder.GetExchange(typeof(T)),
                    routingKey: _conventionsBuilder.GetRoutingKey(typeof(T)),
                    basicProperties: null,
                    body: body);
            }
            return Task.CompletedTask;
        }
    }
}