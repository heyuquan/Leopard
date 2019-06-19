using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Leopard.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Leopard.Dependency
{
    public static class ServiceDescriptorConvert
    {
        /// <summary>
        /// 获取服务描述信息
        /// </summary>
        /// <param name="implementationType">要注册的服务实现类型</param>
        public static List<ServiceInfo> GetServiceDescriptor(this Type implementationType)
        {
            List<ServiceInfo> descriptorLst = new List<ServiceInfo>();

            if (implementationType.IsAbstract || implementationType.IsInterface)
            {
                return descriptorLst; 
            }
            ServiceLifetime? lifetime = GetLifetimeOrNull(implementationType);
            if (lifetime == null)
            {
                return descriptorLst; 
            }

            Type[] serviceTypes = GetImplementedInterfaces(implementationType);

            //服务数量为0时注册自身
            if (serviceTypes.Length == 0)
            {
                descriptorLst.Add(new ServiceInfo(implementationType, implementationType, lifetime.Value));
            }

            //注册服务
            for (int i = 0; i < serviceTypes.Length; i++)
            {
                Type serviceType = serviceTypes[i];
                descriptorLst.Add(new ServiceInfo(serviceType, implementationType, lifetime.Value));
            }

            return descriptorLst;
        }


        /// <summary>
        /// 重写以实现 从类型获取要注册的<see cref="ServiceLifetime"/>生命周期类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>生命周期类型</returns>
        private static ServiceLifetime? GetLifetimeOrNull(Type type)
        {
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
        private static Type[] GetImplementedInterfaces(Type type)
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
    }
}
