using System;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Dapper
{
    public static class DapperServiceCollection
    {
        public static IServiceCollection AddDapper(this IServiceCollection services, Action<DapperOptions> setupAction)
        {
            if(services == null)
                throw new ArgumentNullException(nameof(services));

            if(setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));

            services.AddOptions();
            services.Configure(setupAction);
            services.AddScoped(typeof(DapperBaseConnection));

            return services;
            
        }
        
        
    }
}