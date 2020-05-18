using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Actio.Common.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Actio.Common.RabbitMQ
{
    public class BusSubscribe : IBusSubscribe
    {
        private readonly RabbitMqOptions _options;
        private readonly IConventionsBuilder _conventionsBuilder;
        private readonly IConnection _connection;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public BusSubscribe(
            IOptions<RabbitMqOptions> options,
            IConventionsBuilder conventionsBuilder,
            IConnection connection,
            IServiceScopeFactory serviceScopeFactory)
        {
            _options = options.Value;
            _conventionsBuilder = conventionsBuilder;
            _connection = connection;
        }

        public Task Subscribe<T>() where T : class
        {
            using (var channel = _connection.CreateModel())
            {
                // var exchanges = AppDomain.CurrentDomain
                //     .GetAssemblies()
                //     .SelectMany(a => a.GetTypes())
                //     .Where(t => t.IsDefined(typeof(MessageAttribute), false))
                //     .Select(t => t.GetCustomAttribute<MessageAttribute>().Exchange)
                //     .Distinct()
                //     .ToList();

                if (_options.Exchange.Declare)
                {
                    channel.ExchangeDeclare(
                        exchange: _options.Exchange.Name,
                        type: _options.Exchange.Type,
                        durable: _options.Exchange.Durable,
                        autoDelete: _options.Exchange.AutoDelete
                    );
                }

                // foreach (var exchange in exchanges)
                // {
                //     if (exchange.Equals(_options.Exchange?.Name, 
                //         StringComparison.InvariantCultureIgnoreCase))
                //     {
                //         continue;
                //     }
                //     channel.ExchangeDeclare(exchange, "topic", true);
                // }


                channel.QueueBind(
                    queue: _conventionsBuilder.GetQueue(typeof(T)),
                    exchange: _conventionsBuilder.GetExchange(typeof(T)),
                    routingKey: _conventionsBuilder.GetRoutingKey(typeof(T)));
                channel.BasicQos(
                    _options.Qos.PrefetchSize,
                    _options.Qos.PrefetchCount,
                    _options.Qos.Global);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());

                    var handler = _serviceScopeFactory.CreateScope()
                        .ServiceProvider.GetService(typeof(IEventHandler<>));
                    var eventType = typeof(T);
                    var conreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                    var @event = JsonConvert.DeserializeObject(message, eventType);
                    await (Task)conreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                };

                channel.BasicConsume(
                   queue: _conventionsBuilder.GetQueue(typeof(T)),
                   autoAck: true,
                   consumer: consumer);
            }
            return Task.CompletedTask;
        }
    }
}