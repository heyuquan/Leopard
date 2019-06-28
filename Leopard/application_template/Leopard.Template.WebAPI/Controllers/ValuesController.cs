using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Leopard.Template.WebAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;

namespace Leopard.Template.WebAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new string[] { "cache" })]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value3" };
        }
    }
}

namespace Leopard.Template.WebAPI.Controllers.v2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/values")]
    [ResponseCache(CacheProfileName = "Never")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {            
            List<string> hashLst = new List<string>();
            for (int i = 0; i <= 3; i++)
            {
                LocalSystemClock item = Startup.defaultPool.Get();
                hashLst.Add(item.GetHashCode().ToString());
                Startup.defaultPool.Return(item);
            }

            return hashLst;
        }
    }
}
