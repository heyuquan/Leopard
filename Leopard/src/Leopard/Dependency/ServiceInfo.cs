using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leopard.Dependency
{
    /// <summary>
    /// 服务描述信息（注入，拦截等）
    /// </summary>
    public class ServiceInfo
    {
        public ServiceInfo(Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            this.ServiceType = serviceType;
            this.ImplementationType = implementationType;
            this.Lifetime = lifetime;
        }

      
        public Type ImplementationType { get; private set; }
        
        public Type ServiceType { get; private set; }
        
        public ServiceLifetime Lifetime { get; private set; }
        

    }
}
