﻿using System;
using System.Collections.Generic;
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
        /// 是否为空
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string source)
        {
            return !string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成
        /// </summary>
        /// <param name="source">要测试的字符串</param>
        /// <returns>如果 value 参数为 null 或 System.String.Empty，或者如果 value 仅由空白字符组成，则为 true。</returns>
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        /// 字符串是否为Null或Empty或WhiteSpace
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns>是否为Null或Empty或WhiteSpace</returns>
        public static bool IsNullOrEmptyOrWhiteSpace(this string source)
        {
            return string.IsNullOrEmpty(source) || string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        /// 是否匹配相等
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="pattern">相比较字符串</param>
        /// <returns></returns>
        public static bool IsMatch(this string source, string pattern)
        {
            return !source.IsNullOrEmpty() && Regex.IsMatch(source, pattern);
        }

        /// <summary>
        /// 相同的字符串
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="pattern">相比较字符串</param>
        /// <returns>相同的字符串</returns>
        public static string Match(this string source, string pattern)
        {
            string result = source.IsNullOrEmpty() ? "" : Regex.Match(source, pattern).Value;
            return result;
        }

        /// <summary>
        /// 是否是url地址
        /// </summary>
        /// <param name="checkStr">字符串</param>
        /// <returns></returns>
        public static bool IsUrlAddress(this string checkStr)
        {
            return !checkStr.IsNullOrEmpty() && Regex.IsMatch(checkStr, @"[a-zA-z]+://[^s]*", RegexOptions.Compiled);
        }

        /// <summary>
        /// 判断是否是正确的电子邮件格式
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns>bool</returns>
        public static bool IsEmail(this string source)
        {
            return Regex.IsMatch(source, @"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$", RegexOptions.Compiled);
        }
        /// <summary>
        /// 判断是否是正确的身份证编码格式
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns>bool</returns>
        public static bool IsIdCard(this string source)
        {
            return Regex.IsMatch(source, @"^\d{17}(\d|x)$|^\d{15}$", RegexOptions.Compiled);
        }
        /// <summary>
        /// 判断是否是15位身份证号
        /// </summary>
        /// <param name="id">身份证号</param>
        /// <param name="mesage">返回结果信息</param>
        /// <returns></returns>
        public static bool IsIdCard15(this string id, out string mesage)
        {
            long n = 0;
            if (long.TryParse(id, out n) == false || n < Math.Pow(10, 14))
            {
                mesage = "不是有效的身份证号";
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(id.Remove(2)) == -1)
            {
                mesage = "省份不合法";
                return false;//省份验证
            }
            string birth = id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                mesage = "生日不合法";
                return false;//生日验证
            }
            mesage = "正确";
            return true;//符合15位身份证标准
        }

        /// <summary>
        /// 判断是否是正确的邮政编码格式
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsPostcode(this string source)
        {
            return Regex.IsMatch(source, @"^[1-9]{1}(\d){5}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 判断是否是正确的中国移动或联通电话
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsMobilePhone(this string source)
        {
            return Regex.IsMatch(source, @"^(86)*0*13\d{9}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 判断是否是正确的中国固定电话
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsTelephone(this string source)
        {
            return Regex.IsMatch(source, @"^((\d{3,4})|\d{3,4}-|\s)?\d{8}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 包含html标签
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsHasHtml(this string source)
        {
            Regex reg = new Regex(@"<|>");
            return reg.IsMatch(source);
        }

        /// <summary>
        /// 是否匹配正则表达式，匹配返回true，否则false
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="regex">正则表达式</param>
        /// <returns></returns>
        public static bool IsMatchRegex(this string source, string regex)
        {
            Regex r = new Regex(regex);
            return r.IsMatch(source);
        }

        /// <summary>
        /// 判断字符串是否是IP，如果是返回True，不是返回False
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsIp(this string source)
        {
            Regex regex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])"
                + @"\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$", RegexOptions.Compiled);
            return regex.Match(source).Success;
        }

        /// <summary>
        /// 是否包含中文或全角字符
        /// </summary>
        /// <param name="checkStr">字符串</param>
        /// <returns></returns>
        public static bool IsHasChinese(this string checkStr)
        {
            ASCIIEncoding n = new ASCIIEncoding();
            byte[] b = n.GetBytes(checkStr);
            for (int i = 0; i <= b.Length - 1; i++)
                if (b[i] == 63) return true;  //判断是否(T)为汉字或全脚符号 
            return false;
        }

        /// <summary>
        /// 是否是中文
        /// </summary>
        /// <param name="checkStr">字符串</param>
        /// <returns></returns>
        public static bool IsAllChinese(this string checkStr)
        {
            checkStr = checkStr.Trim();
            if (checkStr == string.Empty) return false;
            Regex reg = new Regex(@"^([\u4e00-\u9fa5]*)$", RegexOptions.Compiled);
            return reg.IsMatch(checkStr);
        }

        /// <summary>
        /// 是否为正整数
        /// </summary>
        /// <param name="intStr">字符串</param>
        /// <returns></returns>
        public static bool IsInt(this string intStr)
        {
            Regex regex = new Regex("^\\d+$", RegexOptions.Compiled);
            return regex.IsMatch(intStr.Trim());
        }

        /// <summary>
        /// 非负整数
        /// </summary>
        /// <param name="intStr">字符串</param>
        /// <returns></returns>
        public static bool IsIntWithZero(this string intStr)
        {
            return Regex.IsMatch(intStr, @"^\\d+$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <param name="checkStr">字符串</param>
        /// <returns></returns>
        public static bool IsNumber(this string checkStr)
        {
            return Regex.IsMatch(checkStr, @"^[+-]?[0123456789]*[.]?[0123456789]*$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 是否是Decimal类型数据
        /// </summary>
        /// <param name="checkStr">字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(this string checkStr)
        {
            return Regex.IsMatch(checkStr, @"^[0-9]+/.?[0-9]{0,2}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 是否是DateTime类型数据
        /// </summary>
        /// <param name="checkStr">字符串</param>
        /// <returns></returns>
        public static bool IsDateTime(this string checkStr)
        {
            return Regex.IsMatch(checkStr, @"^[ ]*[012 ]?[0123456789]?[0123456789]{2}[ ]*[-]{1}[ ]*[01]?[0123456789]{1}[ ]*[-]{1}[ ]*[0123]?[0123456789]"
                + @"{1}[ ]*[012]?[0123456789]{1}[ ]*[:]{1}[ ]*[012345]?[0123456789]{1}[ ]*[:]{1}[ ]*[012345]?[0123456789]{1}[ ]*$", RegexOptions.Compiled);
        }
    }
}
