using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Leopard.Extensions
{
    /// <summary>
    /// 字符串方法扩展
    /// </summary>
    public static partial class StringExtentions
    {

        /// <summary>
        /// 驼峰命名
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToHump(this string text)
        {
            string[] array = text.Split('_');
            string res = string.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                res += array[i].ToUpperHead();
            }
            return res;
        }

        /// <summary>
        /// 驼峰命名（不移除 _ ）
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToOnlyHump(this string text)
        {
            string[] array = text.Split('_');
            List<string> newarray = new List<string>();
            for (int i = 0; i < array.Length; i++)
            {
                newarray.Add(array[i].ToUpperHead());
            }
            return newarray.Join("_");
        }

        /// <summary>
        /// 换为首字母大写的字符串
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns>首字母大写的字符串</returns>
        public static string ToUpperHead(this string str)
        {
            if (str.IsNullOrEmptyOrWhiteSpace() || (str[0] >= 'A' && str[0] <= 'Z'))
            {
                return str;
            }
            if (str.Length == 1)
            {
                return str.ToUpper();
            }
            return string.Format("{0}{1}", str[0].ToString().ToUpper(), str.Substring(1).ToLower());
        }

        /// <summary>
        /// 将字符串进行Unicode编码，变成形如“\u7f16\u7801”的形式
        /// </summary>
        /// <param name="source">要进行编号的字符串</param>
        public static string ToUnicodeString(this string source)
        {
            Regex regex = new Regex(@"[^\u0000-\u00ff]");
            return regex.Replace(source, m => string.Format(@"\u{0:x4}", (short)m.Value[0]));
        }

        /// <summary>
        /// 将形如“\u7f16\u7801”的Unicode字符串解码
        /// </summary>
        public static string FromUnicodeString(this string source)
        {
            Regex regex = new Regex(@"\\u([0-9a-fA-F]{4})", RegexOptions.Compiled);
            return regex.Replace(source,
                m =>
                {
                    short s;
                    if (short.TryParse(m.Groups[1].Value, NumberStyles.HexNumber, CultureInfo.InstalledUICulture, out s))
                    {
                        return "" + (char)s;
                    }
                    return m.Value;
                });
        }


        /// <summary>
        /// url进行编码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ToUrlEncode(this string url)
        {
            if (url.IsNullOrEmpty())
            {
                return url;
            }
            return System.Web.HttpUtility.UrlEncode(url);
        }

        /// <summary>
        /// url进行解码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ToUrlDecode(this string url)
        {
            if (url.IsNullOrEmpty())
            {
                return url;
            }
            return System.Web.HttpUtility.UrlDecode(url);
        }


        /// <summary>
        /// 将字符串转换为<see cref="byte"/>[]数组，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        public static byte[] ToBytes(this string value, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding.GetBytes(value);
        }

        /// <summary>
        /// 将<see cref="byte"/>[]数组转换为字符串，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        public static string ToString2(this byte[] bytes, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 将<see cref="byte"/>[]数组转换为Base64字符串
        /// </summary>
        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 将字符串转换为Base64字符串，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        /// <param name="source">正常的字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns>Base64字符串</returns>
        public static string ToBase64String(this string source, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return Convert.ToBase64String(encoding.GetBytes(source));
        }

        /// <summary>
        /// 将Base64字符串转换为正常字符串，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        /// <param name="base64String">Base64字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns>正常字符串</returns>
        public static string FromBase64String(this string base64String, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            byte[] bytes = Convert.FromBase64String(base64String);
            return encoding.GetString(bytes);
        }


        /// <summary>
        /// 字符串转换成bool类型
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool ToBoolean(this string source)
        {
            bool reValue;
            bool.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// 转化为Byte型
        /// </summary>
        /// <returns>Byte</returns>
        public static Byte ToByte(this string source)
        {
            Byte reValue;
            Byte.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// 转化为Short型
        /// </summary>
        /// <returns>Short</returns>
        public static short ToShort(this string source)
        {
            short reValue;
            short.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// 转化为Short型
        /// </summary>
        /// <returns>Short</returns>
        public static short ToInt16(this string source)
        {
            short reValue;
            short.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// 转化为int32型
        /// </summary>
        /// <returns>int32</returns>
        public static int ToInt32(this string source)
        {
            int reValue;
            Int32.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// 转化为int64型
        /// </summary>
        /// <returns>int64</returns>
        public static long ToInt64(this string source)
        {
            long reValue;
            Int64.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// 转化为Double型
        /// </summary>
        /// <returns>decimal</returns>
        public static Double ToDouble(this string source)
        {
            Double reValue;
            Double.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// 转化为decimal型
        /// </summary>
        /// <returns>decimal</returns>
        public static decimal ToDecimal(this string source)
        {
            decimal reValue;
            decimal.TryParse(source, out reValue);
            return reValue;
        }

        /// <summary>
        /// 转化为数字类型的日期
        /// </summary>
        /// <returns>DateTime</returns>
        public static decimal ToDateTimeDecimal(this string source)
        {
            DateTime reValue;
            return DateTime.TryParse(source, out reValue) ? reValue.ToString("yyyyMMddHHmmss").ToDecimal() : 0;
        }

        /// <summary>
        /// HDF
        /// 2009-3-12
        /// 将时间转换成数字
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static decimal ToDateTimeDecimal(this DateTime source)
        {
            return source.ToString("yyyyMMddHHmmss").ToDecimal();
        }

        /// <summary>
        /// 转换成TextArea保存的格式；（textarea中的格式保存显示的时候会失效）
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToTextArea(this string @source)
        {
            return source.IsNullOrEmpty() ? source : source.Replace("\n\r", "<br/>").Replace("\r", "<br>").Replace("\t", "　　");
        }


        /// <summary>
        /// 字符串拼接成的数组转换成集合
        /// </summary>
        /// <param name="arrStr">要转换的字符串</param>
        /// <param name="splitchar">分离字符(默认,)</param>
        /// <returns></returns>
        public static List<int> ToIntList(this string arrStr, char splitchar = ',')
        {
            if (arrStr.IsNullOrEmpty())
            {
                return new List<int>();
            }
            else
            {
                try
                {
                    return arrStr.Split(splitchar).Select(m => m.ToInt32()).ToList();
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message);
                }
            }
        }

        /// <summary>
        /// 将字符串转换成int类型的数组
        /// </summary>
        /// <param name="arrStr">要转换的字符串</param>
        /// <param name="splitchar">分离字符(默认,)</param>
        /// <returns></returns>
        public static int[] ToIntArray(this string arrStr, char splitchar = ',')
        {
            if (arrStr.IsNullOrEmpty())
            {
                return new int[0];
            }
            try
            {
                int[] array = arrStr.Split(splitchar).Select(m => m.ToInt32()).ToArray();
                return array;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

    }
}
