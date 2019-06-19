// -----------------------------------------------------------------------
//  <copyright file="DependencyTypeFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-31 20:53</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Leopard.Extensions;

namespace Leopard.Dependency
{
    /// <summary>
    /// 依赖注入类型查找器
    /// </summary>
    public class DependencyTypeFinder : IDependencyTypeFinder
    {
        private readonly IEnumerable<Assembly> assemblies;

        /// <summary>
        /// 初始化一个<see cref="DependencyTypeFinder"/>类型的新实例
        /// </summary>
        public DependencyTypeFinder(IEnumerable<Assembly> assemblies)
        {
            this.assemblies = assemblies;
        }

        /// <summary>
        /// 查找 ISingletonDependency，IScopeDependency，ITransientDependency，DependencyAttribute，IgnoreDependencyAttribute标示的注入项
        /// </summary>
        public Type[] FindAll()
        {
            Type[] baseTypes = new[] { typeof(ISingletonDependency), typeof(IScopeDependency), typeof(ITransientDependency) };
            Type[] types = assemblies.SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && !type.IsInterface && !type.HasAttribute<IgnoreDependencyAttribute>()
                    && baseTypes.Any(b => b.IsAssignableFrom(type)))
                .ToArray();
            return types;
        }

        /// <summary>
        /// 获取构造函数传入的程序集集合
        /// </summary>
        public IEnumerable<Assembly> GetAssemblies() => this.assemblies;
    }
}