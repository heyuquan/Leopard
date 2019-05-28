using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Leopard.Extensions
{
    /// <summary>
    /// object类型扩展
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Json转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string json) where T : class
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 对象转JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将对象序列化成url参数形式
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string ToUrlParam(this object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] infos = type.GetProperties();
            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo item in infos)
            {
                string name = item.Name;
                object val = item.GetValue(obj, null);
                if (val != null)
                {
                    sb.AppendFormat("{0}={1}&", name, val.ToString());
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// 判断对象不是空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        /// <summary>
        /// 是否存在<see cref="T"/>集合中
        /// </summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <param name="value">要判断的值</param>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static bool In<T>(this T value, IEnumerable<T> list)
        {
            return list.Contains(value);
        }
    }
}