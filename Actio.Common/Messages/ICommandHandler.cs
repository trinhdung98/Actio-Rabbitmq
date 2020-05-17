using System.Threading.Tasks;

namespace Actio.Common.Messages
{
    public interface ICommandHandler<in T> where T : ICommand
    {
         Task HandleAsync(T command);
    }
}