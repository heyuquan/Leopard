using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Leopard.Template.WebAPI.HttpClients
{

    //使用以下方法之一将每个请求状态与消息处理程序共享：
    //      使用 HttpRequestMessage.Properties 将数据传递到处理程序。
    //      使用 IHttpContextAccessor 访问当前请求。
    //      创建自定义 AsyncLocal 存储对象以传递数据。

    public class ValidateHeaderHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
           HttpRequestMessage request,
           CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains("X-API-KEY"))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(
                   "You must supply an API key header called X-API-KEY")
                };
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
