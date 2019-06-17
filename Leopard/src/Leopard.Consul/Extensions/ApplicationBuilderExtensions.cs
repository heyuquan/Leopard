using Consul;
using Leopard;
using Leopard.Consul;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Builder
{
    // 方案来源：
    // Consul 动态服务注册（非硬编码ip、端口）
    // https://msd.misuland.com/pd/13740804143270575
    // 原文：http://michaco.net/blog/ServiceDiscoveryAndHealthChecksInAspNetCoreWithConsul?tag=Consul
    // 示例代码：https://github.com/MichaCo/AspNetCore.Services
    /// <summary>
    /// Consul 中间件扩展
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseConsulRegisterService(this IApplicationBuilder app)
        {
            Check.NotNull(app, nameof(app));

            var lifetime = app.ApplicationServices.GetService(typeof(IApplicationLifetime)) as IApplicationLifetime;
            Check.NotNull(lifetime, nameof(lifetime));

            var serviceDiscoveryOptions = app.ApplicationServices.GetService(typeof(IOptions<ServiceDiscoveryOptions>)) as IOptions<ServiceDiscoveryOptions>;
            var serviceDiscovery = serviceDiscoveryOptions.Value;
            Check.NotNull(serviceDiscovery, nameof(serviceDiscoveryOptions));
           
            var consul = app.ApplicationServices.GetService(typeof(IConsulClient)) as IConsulClient;

            var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("ServiceDiscoveryBuilder");

            if (string.IsNullOrEmpty(serviceDiscovery.ServiceName))
            {
                throw new ArgumentException("Service Name must be configured", nameof(serviceDiscovery.ServiceName));
            }

            IEnumerable<Uri> addresses = null;
            if (serviceDiscoveryOptions.Value.Endpoints != null && serviceDiscovery.Endpoints.Length > 0)
            {
                logger.LogInformation($"Using {serviceDiscovery.Endpoints.Length} configured endpoints for service registration.");
                addresses = serviceDiscovery.Endpoints.Select(p => new Uri(p));
            }
            else
            {
                logger.LogInformation($"Trying to use server.Features to figure out the service endpoints for service registration.");
                var features = app.Properties["server.Features"] as FeatureCollection;
                addresses = features.Get<IServerAddressesFeature>()
                    .Addresses
                    .Select(p => new Uri(p)).ToArray();
            }

            logger.LogInformation($"Found {addresses.Count()} endpoints: {string.Join(",", addresses.Select(p => p.OriginalString))}.");

            foreach (var address in addresses)
            {
                var serviceId = $"{serviceDiscoveryOptions.Value.ServiceName}_{address.Scheme}_{address.Host}:{address.Port}";

                logger.LogInformation($"Registering service {serviceId} for address {address}.");

                var serviceChecks = new List<AgentServiceCheck>();

                if (!string.IsNullOrEmpty(serviceDiscovery.HealthCheck.HealthCheckTemplate))
                {
                    var healthCheckUri = new Uri(address, serviceDiscovery.HealthCheck.HealthCheckTemplate).OriginalString;
                    serviceChecks.Add(new AgentServiceCheck()
                    {
                        Status = HealthStatus.Passing,
                        DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                        Interval = TimeSpan.FromSeconds(serviceDiscovery.HealthCheck.Interval),
                        HTTP = healthCheckUri
                    });

                    logger.LogInformation($"Adding healthcheck for service {serviceId}, checking {healthCheckUri}.");
                }

                var registration = new AgentServiceRegistration()
                {
                    Checks = serviceChecks.ToArray(),
                    Address = address.Host,
                    ID = serviceId,
                    Name = serviceDiscoveryOptions.Value.ServiceName,
                    Port = address.Port
                };

                consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

                lifetime.ApplicationStopping.Register(() =>
                {
                    consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
                });
            }

            return app;
        }      
    }
}
