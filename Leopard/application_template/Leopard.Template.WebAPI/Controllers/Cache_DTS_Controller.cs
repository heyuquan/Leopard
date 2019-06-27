using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.SqlServer;

namespace Leopard.Template.WebAPI.Controllers
{
    // SqlServerCache
    // RedisCache

    [Route("api/dts-cache")]
    [ApiController]
    public class Cache_DTS_Controller : ControllerBase
    {
        private IDistributedCache cache;
        public Cache_DTS_Controller(IDistributedCache cache)
        {
            this.cache = cache;
        }

        [HttpGet("settime")]
        public async Task<ActionResult<string>> SetTime()
        {
            var currentTime = DateTime.Now.ToString();
            await this.cache.SetStringAsync("CurrentTime", currentTime);
            await this.cache.SetStringAsync("CurrentTime1", currentTime);
            await this.cache.SetStringAsync("CurrentTime2", currentTime);
            return $"设置时间为{currentTime}";
        }

        [HttpGet("gettime")]
        public async Task<ActionResult<string>> GetTime()
        {
            var currentTime = await this.cache.GetStringAsync("CurrentTime");
            return $"获取时间为{currentTime}";
        }

    }
}