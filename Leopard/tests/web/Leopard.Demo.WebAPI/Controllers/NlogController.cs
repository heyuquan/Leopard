using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Leopard.Demo.WebAPI.Controllers
{
    [Route("api/nlog")]
    [ApiController]
    public class NlogController : ControllerBase
    {
        ILogger<NlogController> logger;

        public NlogController(ILogger<NlogController> _logger)
        {
            this.logger = _logger;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
           
            logger.LogInformation("LogInformation，自定义信息");

            try
            {
                MockError();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "LogError,自定义异常");
            }

            return "welcome to NlogController";
        }

        public string MockError()
        {
            throw new Exception("MockError");
        }
    }
}