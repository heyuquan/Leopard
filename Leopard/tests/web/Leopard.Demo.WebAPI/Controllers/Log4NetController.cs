using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using log4net;
using log4net.Core;
using log4net.Repository;

namespace Leopard.Demo.WebAPI.Controllers
{
    // 配置文件层级
    //<root name = "FileLogger" >
    //  < level value="ALL"/>
    //  <appender-ref ref="Debuging"/>
    //  <appender-ref ref="Infoing"/>
    //  <appender-ref ref="WarnLoging"/>
    //  <appender-ref ref="ErrorLoging"/>
    //  <appender-ref ref="FatalLoging"/>
    //</root>

    //<logger name = "FileErrorLogger" >
    //  < level value="ERROR"/>
    //  <appender-ref ref="ErrorLoging"/>
    //</logger>

    [Route("api/log4net")]
    [ApiController]
    public class Log4NetController : ControllerBase
    {
        private log4net.Core.ILogger repositoryLogger;
        private ILog logManagerLogger;

        /// <summary>
        /// 日志信息
        /// </summary>
        ILogger<Log4NetController> logger;

        public Log4NetController(ILogger<Log4NetController> logger)
        {
            this.logger = logger;
            repositoryLogger = Startup.repository.GetLogger("FileErrorLogger");
            logManagerLogger = LogManager.GetLogger(Startup.repository.Name, typeof(Log4NetController));
        }

        [Route("logger")]
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            // 使用微软的 Microsoft.Extensions.Logging.ILogger
            // 使用微软的接口实现方式，不支持层级，只能通过不同的 log4net.config 来实现
            // 获取到的是默认的 root 节点
            this.logger.LogError("常规异常");
            this.logger.LogInformation("info信息");
            this.logger.LogCritical("Critical信息");

            return "welcome to Log4NetController";
        }

        // GET api/values
        [Route("managerlogger")]
        [HttpGet]
        public ActionResult<string> GetLogManagerLogger()
        {
            // LogManager.GetLogger("FileErrorLogger", typeof(LogController));
            // 获取到的是默认的 root 节点
            this.logManagerLogger.Error("常规异常");
            this.logManagerLogger.Info("info信息");
            this.logManagerLogger.Fatal("Critical信息");

            return "welcome to Log4NetController";
        }

        // GET api/values
        [Route("repositorylogger")]
        [HttpGet]
        public ActionResult<string> GetRepositoryLogger()
        {
            // Startup.repository.GetLogger("FileErrorLogger");
            // 可以正确获取到 FileErrorLogger 子节点配置
            this.repositoryLogger.Log(typeof(Log4NetController), log4net.Core.Level.Error, "常规异常", null);
            this.repositoryLogger.Log(typeof(Log4NetController), log4net.Core.Level.Info, "info信息", null);
            this.repositoryLogger.Log(typeof(Log4NetController), log4net.Core.Level.Fatal, "Critical信息", null);

            return "welcome to Log4NetController";
        }

    }
}