﻿// -----------------------------------------------------------------------
//  <copyright file="ITransientDependency.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-16 22:36</last-date>
// -----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

namespace Leopard.Dependency
{
    /// <summary>
    /// 实现此接口的类型将自动注册为<see cref="ServiceLifetime.Transient"/>模式
    /// </summary>
    [IgnoreDependency]
    public interface ITransientDependency
    { }
}