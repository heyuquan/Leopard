﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Leopard.Extensions
{
    /// <summary>
    /// IQueryable集合扩展方法
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// 根据第三方条件是否为真来决定是否执行指定条件的查询
        /// </summary>
        /// <param name="source"> 要查询的源 </param>
        /// <param name="predicate"> 查询条件 </param>
        /// <param name="condition"> 第三方条件 </param>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <returns> 查询的结果 </returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, bool condition)
        {
            Check.NotNull(source, "source");
            Check.NotNull(predicate, "predicate");

            return condition ? source.Where(predicate) : source;
        }
    }
}
