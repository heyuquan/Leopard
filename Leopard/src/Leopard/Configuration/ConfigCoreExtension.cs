﻿using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Leopard.Configuration;

namespace Leopard.Configuration
{
    public static class ConfigCoreExtension
    {

        public static IServiceCollection AddIConfigurationGeter(this IServiceCollection services)
        {
            services.AddSingleton<IConfigurationGeter, ConfigurationGetter>();
            return services;
        }

        /// <summary>
        /// 注入 IConfigDependency 配置文件
        /// </summary>
        public static IServiceCollection AddConfigFinder(this IServiceCollection services)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IConfigDependency))))
                .ToArray();
           
            foreach (var type in types)
            {
                services.AddScoped(type, provider =>
                {
                    var config = provider.GetService<IConfiguration>().GetSection(type.Name).Get(type);                    
                    return config;
                });
            }
            return services;
        }

        public static IConfiguration AddConfigurationGeterLocator(this IConfiguration configuration)
        {
            ConfigurationGeterLocator.SetLocatorProvider(new ConfigurationGetter(configuration));
            return configuration;
        }
    }
}