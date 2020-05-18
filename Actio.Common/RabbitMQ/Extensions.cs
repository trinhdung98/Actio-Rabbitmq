using System.Linq;
using Actio.Common.RabbitMQ.Convention;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Actio.Common.RabbitMQ
{
    public static class Extensions
    {
        private const string SectionName = "rabbitmq";
        public static void AddRabbitMq(this IServiceCollection services, 
            string sectionName = SectionName)
        {
            var options = services.GetOptions<RabbitMqOptions>(sectionName);
            services.AddSingleton(options);

            services.AddTransient<IBusPublish, BusPublish>();
            services.AddTransient<IBusSubscribe, BusSubscribe>();
            services.AddTransient<IConventionsBuilder, ConventionsBuilder>();

            services.AddSingleton(typeof(IServiceScopeFactory));

            var connection = new ConnectionFactory() 
            { 
                UserName = options.UserName,
                Password = options.Password,
                Port = options.Port 
            }.CreateConnection(options.HostNames.ToList(), options.ConnectionName);
            services.AddSingleton(connection);
        }


    }
}