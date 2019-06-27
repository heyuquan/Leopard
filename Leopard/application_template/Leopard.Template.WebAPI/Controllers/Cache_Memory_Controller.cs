using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Leopard.Template.WebAPI.Controllers
{
    [Route("api/memory-cache")]
    [ApiController]
    public class Cache_Memory_Controller : ControllerBase
    {
        private IMemoryCache cache;

        public Cache_Memory_Controller(IMemoryCache cache)
        {
            this.cache = cache;
        }

        [Route("set")]
        [HttpGet]
        public ActionResult<string> Set()
        {
            var id = Guid.NewGuid().ToString();
            cache.Set("userId", id);
            return $"设置的ID：{id}";
        }

        [Route("get")]
        [HttpGet]
        public ActionResult<string> get()
        {
            return cache.Get<string>("userId") ?? "没有设置缓存";
        }

        [Route("set-with-priority")]
        [HttpGet]
        public ActionResult<string> SetWithPriority()
        {
            MemoryCacheEntryOptions entry = new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.NeverRemove
            };
            var id = Guid.NewGuid().ToString();
            cache.Set("userId", id);
            return new JsonResult(entry);
        }

        // 过期通知
        private static void DependentEvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Key:{0} 已过期，依赖于该 Key 的所有缓存都将过期而处于不可用状态", key);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        #region 依赖注入

        private string Dependent_Key = "DEPENDENT_KEY";
        /// <summary>
        /// 设置依赖缓存
        /// </summary>
        /// <returns></returns>
        [Route("set-dependent")]
        [HttpGet]
        public ActionResult<string> SetDependent()
        {
            var cts = new CancellationTokenSource();
            cache.Set<CancellationTokenSource>(Dependent_Key, cts);
            using (var entry = cache.CreateEntry($"{Dependent_Key}_user_session"))
            {
                entry.Value = "id_123424";
                entry.RegisterPostEvictionCallback(DependentEvictionCallback, this);
                cache.Set($"{Dependent_Key}_share_data", "这里是共享的数据", new CancellationChangeToken(cts.Token));
            }

            return "设置依赖完成";
        }

        [Route("get-dependent")]
        [HttpGet]
        public ActionResult<string> GetDependent()
        {
            var userInfo = new
            {
                UserSession = cache.Get<string>($"{Dependent_Key}_user_session"),
                UserShareData = cache.Get<string>($"{Dependent_Key}_share_data"),
            };

            return new JsonResult(userInfo);
        }

        [Route("del-dependent")]
        [HttpGet]
        public ActionResult<string> DelDependent()
        {
            cache.Get<CancellationTokenSource>(Dependent_Key).Cancel();

            var userInfo = new
            {
                UserSession = cache.Get<string>($"{Dependent_Key}_user_session"),
                UserShareData = cache.Get<string>($"{Dependent_Key}_share_data"),
            };

            return new JsonResult(userInfo);
        }

        #endregion
    }
}