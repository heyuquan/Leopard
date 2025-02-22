﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Leopard.Extensions
{
    /// <summary>
    /// object类型扩展
    /// </summary>
    public static class ObjectExtensions
    {
        #region JSON

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

        #endregion

        #region 类型转换

        /// <summary>
        /// 把对象类型转换为指定类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object CastTo(this object value, Type conversionType)
        {
            if (value == null)
            {
                return null;
            }
            if (conversionType.IsNullableType())
            {
                conversionType = conversionType.GetUnNullableType();
            }
            if (conversionType.IsEnum)
            {
                return Enum.Parse(conversionType, value.ToString());
            }
            if (conversionType == typeof(Guid))
            {
                return Guid.Parse(value.ToString());
            }
            return Convert.ChangeType(value, conversionType);
        }

        /// <summary>
        /// 把对象类型转化为指定类型
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <returns> 转化后的指定类型的对象，转化失败引发异常。 </returns>
        public static T CastTo<T>(this object value)
        {
            if (value == null && default(T) == null)
            {
                return default(T);
            }
            if (value.GetType() == typeof(T))
            {
                return (T)value;
            }
            object result = CastTo(value, typeof(T));
            return (T)result;
        }

        /// <summary>
        /// 把对象类型转化为指定类型，转化失败时返回指定的默认值
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <param name="defaultValue"> 转化失败返回的指定默认值 </param>
        /// <returns> 转化后的指定类型对象，转化失败时返回指定的默认值 </returns>
        public static T CastTo<T>(this object value, T defaultValue)
        {
            try
            {
                return CastTo<T>(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        #endregion

        /// <summary>
        /// 再进行Get请求时，调用此方法将对象转化为url参数(多个&符号连接，不带?)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="masterParamName">目的接口参数名字</param>
        /// <returns></returns>
        public static string ToUrlParams(this object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder(32);

            Type type = obj.GetType();

            {
                //获取公共属性    
                PropertyInfo[] propertys = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

                for (int i = 0; i < propertys.Length; i++)
                {
                    PropertyInfo pi = type.GetProperty(propertys[i].Name);
                    object val = pi.GetValue(obj);
                    if (val != null)
                    {
                        if (IsBaseValueType(val.GetType()))
                        {
                            if (sb.Length == 0)
                            {
                                sb.AppendFormat("{0}={1}", pi.Name, val);
                            }
                            else
                            {
                                sb.Append("&").AppendFormat("{0}={1}", pi.Name, val);
                            }
                        }
                        else
                        {
                            if (sb.Length == 0)
                            {
                                sb.Append(val.InnerToUrlParams(pi.Name));
                            }
                            else
                            {
                                sb.Append("&").Append(val.InnerToUrlParams(pi.Name));
                            }
                        }
                    }
                }
            }
            return sb.ToString();
        }

        private static string InnerToUrlParams(this object obj, string masterParamName)
        {
            if (obj == null)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder(32);

            Type type = obj.GetType();

            {
                //获取公共属性    
                PropertyInfo[] propertys = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

                for (int i = 0; i < propertys.Length; i++)
                {
                    // Propertys[i].SetValue(Propertys[i], i, null); //设置值    
                    PropertyInfo pi = type.GetProperty(propertys[i].Name);
                    object val = pi.GetValue(obj);
                    if (val != null)
                    {
                        if (IsBaseValueType(val.GetType()))
                        {
                            if (sb.Length == 0)
                            {
                                sb.AppendFormat("{0}.{1}={2}", masterParamName, pi.Name, val);
                            }
                            else
                            {
                                sb.Append("&").AppendFormat("{0}.{1}={2}", masterParamName, pi.Name, val);
                            }
                        }
                        else
                        {
                            if (sb.Length == 0)
                            {
                                sb.Append(val.InnerToUrlParams(string.Format("{0}.{1}", masterParamName, pi.Name)));
                            }
                            else
                            {
                                sb.Append("&").Append(val.InnerToUrlParams(string.Format("{0}.{1}", masterParamName, pi.Name)));
                            }
                        }
                    }
                }
            }
            return sb.ToString();
        }

        private static bool IsBaseValueType(Type type)
        {
            return type.IsEquivalentTo(typeof(string))
                          || type.IsEquivalentTo(typeof(byte))
                          || type.IsEquivalentTo(typeof(sbyte))
                          || type.IsEquivalentTo(typeof(short))
                          || type.IsEquivalentTo(typeof(int))
                          || type.IsEquivalentTo(typeof(short))
                          || type.IsEquivalentTo(typeof(long))
                          || type.IsEquivalentTo(typeof(double))
                          || type.IsEquivalentTo(typeof(float))
                          || type.IsEquivalentTo(typeof(decimal))
                          || type.IsEquivalentTo(typeof(byte?))
                          || type.IsEquivalentTo(typeof(sbyte?))
                          || type.IsEquivalentTo(typeof(short?))
                          || type.IsEquivalentTo(typeof(int?))
                          || type.IsEquivalentTo(typeof(short?))
                          || type.IsEquivalentTo(typeof(long?))
                          || type.IsEquivalentTo(typeof(double?))
                          || type.IsEquivalentTo(typeof(float?))
                          || type.IsEquivalentTo(typeof(decimal?));
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

        /// <summary>
        /// 对象深度拷贝，复制出一个数据一样，但地址不一样的新版本
        /// </summary>
        public static T DeepClone<T>(this T obj) where T : class
        {
            if (obj == null)
            {
                return default(T);
            }
            if (typeof(T).HasAttribute<SerializableAttribute>())
            {
                throw new NotSupportedException(string.Format("当前对象未标记特性“{0}”，无法进行DeepClone操作", typeof(SerializableAttribute)));
            }
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, obj);
                ms.Seek(0L, SeekOrigin.Begin);
                return (T)formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// 转化为 byte[]
        /// </summary>
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}