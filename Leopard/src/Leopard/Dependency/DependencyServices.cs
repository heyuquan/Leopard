﻿using System;
using System.Linq;
using System.Reflection;
using Leopard.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Leopard.Dependency
{
    public class DependencyServices
    {
        /// <summary>
        /// 将服务实现类型注册到服务集合中
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="implementationType">要注册的服务实现类型</param>
        public virtual void AddToServices(IServiceCollection services, Type implementationType)
        {
            if (implementationType.IsAbstract || implementationType.IsInterface)
            {
                return;
            }
            ServiceLifetime? lifetime = this.GetLifetimeOrNull(implementationType);
            if (lifetime == null)
            {
                return;
            }
            DependencyAttribute dependencyAttribute = implementationType.GetAttribute<DependencyAttribute>();
            Type[] serviceTypes = this.GetImplementedInterfaces(implementationType);

            //服务数量为0时注册自身
            if (serviceTypes.Length == 0)
            {
                services.TryAdd(new ServiceDescriptor(implementationType, implementationType, lifetime.Value));
                return;
            }

            //服务实现显示要求注册身处时，注册自身并且继续注册接口
            if (dependencyAttribute?.AddSelf == true)
            {
                services.TryAdd(new ServiceDescriptor(implementationType, implementationType, lifetime.Value));
            }

            //注册服务
            for (int i = 0; i < serviceTypes.Length; i++)
            {
                Type serviceType = serviceTypes[i];
                ServiceDescriptor descriptor = new ServiceDescriptor(serviceType, implementationType, lifetime.Value);
                if (lifetime.Value == ServiceLifetime.Transient)
                {
                    services.TryAddEnumerable(descriptor);
                    continue;
                }

                bool multiple = serviceType.HasAttribute<MultipleDependencyAttribute>();
                if (i == 0)
                {
                    if (multiple)
                    {
                        services.Add(descriptor);
                    }
                    else
                    {
                        AddSingleService(services, descriptor, dependencyAttribute);
                    }
                }
                else
                {
                    //有多个接口，后边的接口注册使用第一个接口的实例，保证同个实现类的多个接口获得同一实例
                    Type firstServiceType = serviceTypes[0];
                    descriptor = new ServiceDescriptor(serviceType, provider => provider.GetService(firstServiceType), lifetime.Value);
                    if (multiple)
                    {
                        services.Add(descriptor);
                    }
                    else
                    {
                        AddSingleService(services, descriptor, dependencyAttribute);
                    }
                }
            }
        }


        /// <summary>
        /// 重写以实现 从类型获取要注册的<see cref="ServiceLifetime"/>生命周期类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>生命周期类型</returns>
        protected virtual ServiceLifetime? GetLifetimeOrNull(Type type)
        {
            DependencyAttribute attribute = type.GetAttribute<DependencyAttribute>();
            if (attribute != null)
            {
                return attribute.Lifetime;
            }

            if (type.IsDeriveClassFrom<ITransientDependency>())
            {
                return ServiceLifetime.Transient;
            }

            if (type.IsDeriveClassFrom<IScopeDependency>())
            {
                return ServiceLifetime.Scoped;
            }

            if (type.IsDeriveClassFrom<ISingletonDependency>())
            {
                return ServiceLifetime.Singleton;
            }

            return null;
        }

        /// <summary>
        /// 重写以实现 获取实现类型的所有可注册服务接口
        /// </summary>
        /// <param name="type">依赖注入实现类型</param>
        /// <returns>可注册的服务接口</returns>
        protected virtual Type[] GetImplementedInterfaces(Type type)
        {
            Type[] exceptInterfaces = { typeof(IDisposable) };
            Type[] interfaceTypes = type.GetInterfaces().Where(t => !exceptInterfaces.Contains(t) && !t.HasAttribute<IgnoreDependencyAttribute>()).ToArray();
            for (int index = 0; index < interfaceTypes.Length; index++)
            {
                Type interfaceType = interfaceTypes[index];
                if (interfaceType.IsGenericType && !interfaceType.IsGenericTypeDefinition && interfaceType.FullName == null)
                {
                    interfaceTypes[index] = interfaceType.GetGenericTypeDefinition();
                }
            }
            return interfaceTypes;
        }

        private static void AddSingleService(IServiceCollection services,
            ServiceDescriptor descriptor,
            DependencyAttribute dependencyAttribute)
        {
            if (dependencyAttribute?.ReplaceExisting == true)
            {
                services.Replace(descriptor);
            }
            else if (dependencyAttribute?.TryAdd == true)
            {
                services.TryAdd(descriptor);
            }
            else
            {
                services.Add(descriptor);
            }
        }
    }
}
