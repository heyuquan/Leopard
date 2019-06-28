using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedLockNet;

namespace Leopard.Template.WebAPI.Controllers
{
    [Route("api/redlock")]
    [ApiController]
    public class RedLockController : ControllerBase
    {
        private readonly IDistributedLockFactory _distributedLockFactory = null;
        public RedLockController(IDistributedLockFactory distributedLockFactory)
        {
            this._distributedLockFactory = distributedLockFactory;
        }

        [HttpGet]
        public async Task<bool> DistributedLockTest()
        {
            var productId = "id";
            // resource 锁定的对象
            // expiryTime 锁定过期时间，锁区域内的逻辑执行如果超过过期时间，锁将被释放
            // waitTime 等待时间,相同的 resource 如果当前的锁被其他线程占用,最多等待时间
            // retryTime 等待时间内，多久尝试获取一次
            using (var redLock = await this._distributedLockFactory.CreateLockAsync(productId, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(0)))
            {
                if (redLock.IsAcquired)
                {
                    await Task.Delay(1000*10);
                    return true;
                }
                else
                {
                    Console.WriteLine($"获取锁失败：{DateTime.Now}");
                }
            }
            return false;
        }
    }
}