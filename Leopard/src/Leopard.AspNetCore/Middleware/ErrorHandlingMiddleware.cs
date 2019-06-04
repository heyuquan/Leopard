using Leopard.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.AspNetCore.Middleware
{
    /// <summary>
    /// 全局异常处理
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception ex)
            {
                var statusCode = context.Response.StatusCode;
                if (ex is ArgumentException)
                {
                    statusCode = 200;
                }

                await this.HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            var statusCode = context.Response.StatusCode;
            var info = string.Empty;
            switch (statusCode)
            {
                case 401:
                    info = "没有权限";
                    break;
                case 404:
                    info = "未找到服务";
                    break;
                case 403:
                    info = "服务器理解本次客户端的请求，但是拒绝执行此请求";
                    break;
                case 500:
                    info = "服务器内部错误，无法完成请求";
                    break;
                case 502:
                    info = "请求错误";
                    break;
                default:
                    info = "内部错误";
                    break;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("url:{0}", context.Request.GetAbsoluteUri())
              .AppendLine()
              .AppendFormat("statusCode:{0}-{1}", statusCode.ToString(), info);
            this.logger.LogError(ex, sb.ToString());

            var result = JsonConvert.SerializeObject(new { error = ex.Message });
            context.Response.ContentType = "application/json;charset=utf-8";
            // context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
