using Leopard.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leopard.DDD.Domain.IRepository
{
    /// <summary>
    /// 课件仓储
    /// </summary>
    public interface ICourseRepository : IScopeDependency
    {
        string GetName();
    }
}
