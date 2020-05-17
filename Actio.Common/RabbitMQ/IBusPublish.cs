using System.Threading.Tasks;
using Actio.Common.Messages;

namespace Actio.Common.RabbitMQ
{
    public interface IBusPublish
    {
        Task PublishAsync<T>(T message) where T : class;
    }
}