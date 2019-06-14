using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiServiceB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsulController : ControllerBase
    {
        static List<string> Urls = new List<string>();

        /// <summary>
        /// 获取注册在Consul中的所有服务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AllNodes()
        {
            Catalog_Nodes().GetAwaiter().GetResult();
            return new JsonResult(Urls);
        }

        public static async Task Catalog_Nodes()
        {
            var client = new ConsulClient();
            var nodeList = await client.Agent.Services();
            var url = nodeList.Response.Values;
           
            foreach (var item in url)
            {
                string address = item.Address;
                int port = item.Port;
                string name = item.Service;
                Urls.Add($"http://{address}:{port}");
            }
        }
    }
}