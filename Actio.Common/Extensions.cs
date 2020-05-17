using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Actio.Common
{
    public static class Extensions
    {
        public static TModel GetOptions<TModel>(this IServiceCollection services, string section) 
            where TModel : new()
        {
            var model = new TModel();
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();
            configuration.GetSection(section).Bind(model);
            return model;
        }
    }
}