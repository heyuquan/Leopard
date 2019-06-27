using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

public class AdminSafeListMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AdminSafeListMiddleware> _logger;
    private readonly string _adminSafeList;

    public AdminSafeListMiddleware(
        RequestDelegate next,
        ILogger<AdminSafeListMiddleware> logger,
        string adminSafeList)
    {
        this._adminSafeList = adminSafeList;
        this._next = next;
        this._logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Method != "GET")
        {
            var remoteIp = context.Connection.RemoteIpAddress;
            this._logger.LogDebug($"Request from Remote IP address: {remoteIp}");

            string[] ip = _adminSafeList.Split(';');

            var bytes = remoteIp.GetAddressBytes();
            var badIp = true;
            foreach (var address in ip)
            {
                var testIp = IPAddress.Parse(address);
                if (testIp.GetAddressBytes().SequenceEqual(bytes))
                {
                    badIp = false;
                    break;
                }
            }

            if (badIp)
            {
                this._logger.LogInformation(
                    $"Forbidden Request from Remote IP address: {remoteIp}");
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
        }

        await this._next.Invoke(context);

    }
}