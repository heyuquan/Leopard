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
using System.Collections.Generic;
using Microsoft.Extensions.DependencyModel;
using System.IO;
using System.Runtime.Loader;

namespace Leopard.AspNetCore.Autofac.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// 获取 Autofac 的 IServiceProvider
        /// </summary>
        public static IServiceProvider GetAutofacServiceProvider(this IServiceCollection services)
        {
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
            builder.RegisterTypes(feature.Controllers.Select(ti => ti.AsType()).ToArray())
                .PropertiesAutowired();

            DependencyRegister.Custom(builder);

            builder.Populate(services);
            return new AutofacServiceProvider(builder.Build());
        }

    }
}
