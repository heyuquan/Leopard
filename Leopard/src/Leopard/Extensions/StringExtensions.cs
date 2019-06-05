using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Leopard.Extensions
{
    /// <summary>
    /// 字符串方法扩展
    /// </summary>
    public static partial class StringExtentions
    {

        /// <summary>
        /// 删除字符串头部和尾部的回车/换行/空格
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>清除回车/换行/空格之后的字符串</returns>
        public static string TrimBlank(this string str)
        {
            if (str.IsNullOrEmpty())
            {
                throw new NullReferenceException("字符串不可为空");
            }
            return str.TrimLeft().TrimRight();
        }

        /// <summary>
        /// 删除字符串尾部的回车/换行/空格
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>清除回车/换行/空格之后的字符串</returns>
        public static string TrimRight(this string str)
        {
            if (!str.IsNullOrEmpty())
            {
                int i = 0;
                while ((i = str.Length) > 0)
                {
                    if (!str[i - 1].Equals(' ') && !str[i - 1].Equals('\r') && !str[i - 1].Equals('\n'))
                    {
                        break;
                    }
                    str = str.Substring(0, i - 1);
                }
            }
            return str;
        }

        /// <summary>
        /// 删除字符串头部的回车/换行/空格
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>清除回车/换行/空格之后的字符串</returns>
        public static string TrimLeft(this string str)
        {
            if (!str.IsNullOrEmpty())
            {
                while (str.Length > 0)
                {
                    if (!str[0].Equals(' ') && !str[0].Equals('\r') && !str[0].Equals('\n'))
                    {
                        break;
                    }
                    str = str.Substring(1);
                }
            }
            return str;
        }

        /// <summary>
        /// 相同字符串的数量
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="pattern">相比较字符串</param>
        /// <returns></returns>
        public static int MatchesCount(this string source, string pattern)
        {
            var result = source.IsNullOrEmpty() ? 0 : Regex.Matches(source, pattern).Count;
            return result;
        }

        /// <summary>
        /// 获取字符串长度，按中文2位，英文1位进行计算
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static int CharCodeLength(string source)
        {
            var n = 0;
            foreach (var c in source.ToCharArray())
            {
                if (c < 128)
                    n++;
                else
                    n += 2;
            }

            return n;
        }


        #region Substring扩展

        /// <summary>
        /// SubString方法扩展
        /// </summary>
        /// <param name="str">截取字符串</param>
        /// <param name="length">要截取的长度</param>
        /// <returns>string</returns>
        public static string Substring(this string str, int length)
        {
            if (string.IsNullOrEmpty(str) || str.Length <= length)
            {
                return str;
            }
            return str.Substring(0, length);
        }

        /// <summary>
        /// 截取字符并显示...符号
        /// </summary>
        /// <param name="str">截取字符串</param>
        /// <param name="length">要截取的长度</param>
        /// <returns>string</returns>
        public static string SubstringToSx(this string str, int length)
        {
            if (string.IsNullOrEmpty(str) || str.Length <= length)
            {
                return str;
            }
            return str.Substring(0, length) + "...";
        }

        /// <summary>
        /// 根据某个字符截取
        /// </summary>
        /// <param name="text">要截取的字符串</param>
        /// <param name="delimiter">字符</param>
        /// <returns></returns>
        public static string SubstringUpToFirst(this string text, char delimiter)
        {
            if (text == null)
            {
                return null;
            }
            var num = text.IndexOf(delimiter);
            if (num >= 0)
            {
                return text.Substring(0, num);
            }
            return text;
        }

        #endregion

        /// <summary>
        /// 反射获取属性值
        /// </summary>
        /// <typeparam name="T">匿名对象</typeparam>
        /// <param name="t">匿名对象集合</param>
        /// <param name="propertyname">属性名</param>
        /// <returns></returns>
        public static string GetPropertyValue<T>(this T t, string propertyname)
        {
            Type type = typeof(T);
            PropertyInfo property = type.GetProperty(propertyname);

            if (property == null) return string.Empty;

            object o = property.GetValue(t, null);

            if (o == null) return string.Empty;

            return o.ToString();
        }       

        private static string[] strs =
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v","w", "x", "y", "z",
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V","W", "X", "Y", "Z"
        };

        /// <summary>
        /// 创建伪随机字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strleg">长度</param>
        /// <returns></returns>
        public static string CreateNonce(this string str, long strleg = 15)
        {
            Random r = new Random();
            StringBuilder sb = new StringBuilder();
            var length = strs.Length;
            for (int i = 0; i < strleg; i++)
            {
                sb.Append(strs[r.Next(length - 1)]);
            }
            return sb.ToString();
        }

        private static string[] nums =
        {
            "0","1","2","3","4","5","6","7","8","9"
        };

        /// <summary>
        /// 创建伪随机数字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="numleg"></param>
        /// <returns></returns>
        public static string CreateNumberNonce(this string str, int numleg = 4)
        {
            Random r = new Random();
            StringBuilder sb = new StringBuilder();
            var length = nums.Length;
            for (int i = 0; i < numleg; i++)
            {
                sb.Append(nums[r.Next(length - 1)]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 移除换行
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveLine(this string str)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }
            return str.Replace("\r", "").Replace("\n", "");
        }

    }
}