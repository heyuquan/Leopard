using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Reflection;
using System.Linq;
using Leopard.Dependency;

namespace Leopard.Consul.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加服务注册中心
        /// </summary>
        public static IServiceProvider AddAutofacDependencyServices(this IServiceCollection services)
        {
            // 添加框架自动化依赖接口类
            services.AddDependencyServices();

            // 接管 Controller 注入流程，实现Controller中的属性注入
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            var assembly = Assembly.GetEntryAssembly(); 
            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(assembly));
            manager.FeatureProviders.Add(new ControllerFeatureProvider());
            var feature = new ControllerFeature();
            manager.PopulateFeature(feature);

            var builder = new ContainerBuilder();
            builder.RegisterType<ApplicationPartManager>().AsSelf().SingleInstance();
            builder.RegisterTypes(feature.Controllers.Select(ti => ti.AsType()).ToArray()).PropertiesAutowired();
            builder.Populate(services);

            return new AutofacServiceProvider(builder.Build());
        }
    }
}
