using Leopard.Dependency;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Linq;
using System.IO;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Leopard.AspNetCore.Autofac
{
    public class DependencyRegister
    {
        public static void Custom(ContainerBuilder builder)
        {
            List<Assembly> allAssemblies = new List<Assembly>();

            var dlls = DependencyContext.Default.CompileLibraries
                .SelectMany(x => x.ResolveReferencePaths())
                .Distinct()
                .Where(x => x.Contains(Directory.GetCurrentDirectory()))
                .ToList();
            foreach (var item in dlls)
            {
                try
                {
                    allAssemblies.Add(AssemblyLoadContext.Default.LoadFromAssemblyPath(item));
                }
                catch (System.IO.FileLoadException loadEx)
                {
                } // The Assembly has already been loaded.
                catch (BadImageFormatException imgEx)
                {
                } // If a BadImageFormatException exception is thrown, the file is not an assembly.
                catch (Exception ex)
                {
                }
            }

            List<ServiceInfo> ServiceDescriptorLst = new List<ServiceInfo>();
            //查找所有自动注册的服务实现类型
            IDependencyTypeFinder dependencyTypeFinder = new DependencyTypeFinder(allAssemblies);

            Type[] dependencyTypes = dependencyTypeFinder.FindAll();
            foreach (Type dependencyType in dependencyTypes)
            {
                ServiceDescriptorLst.AddRange(dependencyType.GetServiceDescriptor());
            }

            foreach (var item in ServiceDescriptorLst)
            {
                if (item.Lifetime == ServiceLifetime.Scoped)
                {
                    builder.RegisterType(item.ImplementationType).As(item.ServiceType)
                        .InstancePerLifetimeScope().PropertiesAutowired();
                }
                else if (item.Lifetime == ServiceLifetime.Singleton)
                {
                    builder.RegisterType(item.ImplementationType).As(item.ServiceType)
                        .SingleInstance().PropertiesAutowired();
                }
                else if (item.Lifetime == ServiceLifetime.Transient)
                {
                    builder.RegisterType(item.ImplementationType).As(item.ServiceType)
                      .InstancePerDependency().PropertiesAutowired();
                }
            }
        }
    }
}
