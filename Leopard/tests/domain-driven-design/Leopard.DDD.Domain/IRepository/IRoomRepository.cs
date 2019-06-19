using Leopard.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leopard.DDD.Domain.IRepository
{
    /// <summary>
    /// 教室仓储
    /// </summary>
    //[IgnoreDependency]
    public interface IRoomRepository : IScopeDependency
    {
        string GetName();
    }
}
