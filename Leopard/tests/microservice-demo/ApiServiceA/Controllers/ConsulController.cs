using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiServiceA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsulController : ControllerBase
    {
        /// <summary>
        /// 获取注册在Consul中的所有服务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AllNodes()
        {
            var client = new ConsulClient();
            var nodeList = await client.Agent.Services();
            var data = nodeList.Response.Values.Select(p =>
            {
                return new
                {
                    ID = p.ID,
                    ServiceName = p.Service,
                    Address = p.Address,
                    Port = p.Port
                };
            });
            return new JsonResult(data);
        }
    }
}