using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Actio.Common.RabbitMQ
{
    public static class Extensions
    {
        private const string SectionName = "rabbitmq";
        public static void AddRabbitMq(this IServiceCollection services, 
            string sectionName = SectionName)
        {
            
        }


    }
}