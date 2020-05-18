using System.Threading.Tasks;

namespace Actio.Common.RabbitMQ
{
    public interface IBusSubscribe
    {
        Task Subscribe<T>() where T : class;
    }
}