using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Linq;

namespace Leopard.AspNetCore.Extensions
{
    /// <summary>
    /// HttpRequest扩展
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// 获取请求的绝对路径地址
        /// </summary>
        public static string GetAbsoluteUri(this HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
        }

        /// <summary>
        /// 是否是AJAX请求
        /// </summary>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            return request.Headers != null &&
                   request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        /// <summary>
        /// 获取本地IP 地址:端口
        /// </summary>
        /// <returns>地址:端口</returns>
        [Obsolete("验证是否可行")]
        public static string GetLocalEndpoint(this HttpRequest request)
        {
            return request.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString() + ":" + request.HttpContext.Connection.LocalPort;
        }

        /// <summary>
        /// 获取远程IP
        /// </summary>
        /// <returns>IP</returns>
        [Obsolete("验证是否可行")]
        public static string GetRemoteIp(this HttpRequest request)
        {
            var ip = request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = request.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
    }
}
