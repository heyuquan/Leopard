using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Leopard.Demo.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging((context, logger) =>
                {
                    // 添加过滤。Leopard.Demo.WebAPI.Controllers.LogController 类中的日志输出级别的Error，即info、debug等方法被调用，也不会有日志输出
                    // logger.AddFilter("Leopard.Demo.WebAPI.Controllers.LogController", LogLevel.Error);

                    // 默认加载 log4net.config 配置文件的 root 节点
                    logger.AddLog4Net();
                });
    }
}
