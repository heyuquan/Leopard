using Consul;
using Leopard;
using Leopard.Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加服务注册中心
        /// </summary>
        public static IServiceCollection AddServiceDiscovery(this IServiceCollection services)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            IConfiguration configuration = serviceProvider.GetService<IConfiguration>();
            Check.NotNull(configuration, nameof(configuration));

            services.Configure<ServiceDiscoveryOptions>(configuration.GetSection("ServiceDiscovery"));
            services.AddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;

                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));

            return services;
        }
    }
}
