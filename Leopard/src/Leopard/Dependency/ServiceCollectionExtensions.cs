using Microsoft.Extensions.DependencyInjection;
using System;

namespace Leopard.Dependency
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加服务注册中心
        /// </summary>
        public static IServiceCollection AddDependencyServices(this IServiceCollection services)
        {
            //查找所有自动注册的服务实现类型
            IDependencyTypeFinder dependencyTypeFinder = new DependencyTypeFinder(AppDomain.CurrentDomain.GetAssemblies());

            DependencyServices dependencyServices=new DependencyServices();
            Type[] dependencyTypes = dependencyTypeFinder.FindAll();
            foreach (Type dependencyType in dependencyTypes)
            {
                dependencyServices.AddToServices(services, dependencyType);
            }

            return services;
        }       
    }
}
