using System.Text;
using System.Threading.Tasks;
using Actio.Common.Messages;
using RabbitMQ.Client;

namespace Actio.Common.RabbitMQ
{
    public class BusPublish : IBusPublish
    {
        // public Task PublishAsync<T>(T @event) where T : IEvent
        // {
        //     var factory = new ConnectionFactory() { HostName = "localhost" };
        //     using (var connection = factory.CreateConnection())
        //     using (var channel = connection.CreateModel())
        //     {
        //         channel.ExchangeDeclare(exchange: "topic_logs",
        //                                 type: "topic");

        //         var routingKey = (args.Length > 0) ? args[0] : "anonymous.info";
        //         var body = Encoding.UTF8.GetBytes(message);
        //         channel.BasicPublish(exchange: "topic_logs",
        //                              routingKey: routingKey,
        //                              basicProperties: null,
        //                              body: body);
        //     }
        // }
        public Task PublishAsync<T>(T message) where T : class
        {
            throw new System.NotImplementedException();
        }
    }
}