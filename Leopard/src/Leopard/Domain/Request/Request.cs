using Leopard.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Leopard.Domain.Request
{

    /// <summary>
    /// Service 請求實體
    /// </summary> 
    public class Request
    {
        /// <summary>
        /// 請求頭
        /// </summary>
        public RequestHeader Header { get; set; }

        /// <summary>
        /// 请求数据(RSA加密)
        /// </summary>
        public string[] Data { get; set; }

        /// <summary>
        /// 請求內容類型
        /// </summary>
        public string ContentType { get; set; }

    }


    /// <summary>
    /// Service 請求實體
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Request<T>
    {
        /// <summary>
        /// 請求頭
        /// </summary>
        public RequestHeader Header { get; set; }

        /// <summary>
        /// 請求數據
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 請求內容類型
        /// </summary>
        public string ContentType { get; set; }

    }

    /// <summary>
    /// 請求頭實體
    /// </summary>
    public class RequestHeader
    {
        /// <summary>
        /// 用于唯一标识客户端
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// client version，APP版本号
        /// </summary>
        public string ClientVersion { get; set; }

        /// <summary>
        /// webapp 版本号
        /// </summary>
        public string ServerVersion { get; set; }

        /// <summary>
        /// 系統标示Code
        /// </summary>
        public string SystemCode { get; set; }

        /// <summary>
        /// 登錄用戶Token,沒有登錄時為NULL
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// [营销] 来源，APP=4，H5=5
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// 希望服务端返回的类型
        /// </summary>
        public FormatType AcceptFormat { get; set; } = FormatType.JSON;
    }
}