using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leopard.Template.WebAPI.Controllers
{
    /// <summary>
    /// swagger controller 测试
    /// </summary>
    [Route("api/swagger-test")]
    [ApiExplorerSettings(GroupName = "Swagger组")]
    [ApiController]
    public class SwaggerInfoController : ControllerBase
    {
        /// <summary>
        /// swagger 测试 Get
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "value";
        }

        /// <summary>
        /// swagger 测试 Get2
        /// </summary>
        /// <returns></returns>
        [Route("get2")]
        [HttpGet]
        public ActionResult<string> Get2()
        {
            return "value get2";
        }
    }
}
