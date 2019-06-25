using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Leopard.Template.WebAPI.BackgroundServices
{
    /// <summary>
    /// 后台任务
    /// </summary>
    public class TokenRefreshService : BackgroundService
    {
        private readonly ILogger logger;

        public TokenRefreshService(ILogger<TokenRefreshService> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation("Service doing");

            while (!stoppingToken.IsCancellationRequested)
            {
                this.logger.LogInformation(DateTime.Now.ToLongTimeString() + ":Refresh Token!");
                await Task.Delay(5000, stoppingToken);
            }

            this.logger.LogInformation("service done");
        }
    }
}
