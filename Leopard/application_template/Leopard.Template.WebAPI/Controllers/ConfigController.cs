using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Leopard.Template.WebAPI.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace Leopard.Template.WebAPI.Controllers
{
    [Route("api/config")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration config;

        public ConfigController(IOptionsMonitor<MySetting> optionsAccessor, IConfiguration config)
        {
            this.config = config;

            var a = config.GetSection("MySetting");

            MySetting mySetting = config.GetSection("MySetting").Get<MySetting>();
            MySetting fromStruct = optionsAccessor.CurrentValue;

            // 存在性判断方式
            if (config.GetSection("SetTwo").Exists()) 
            {
                string setTwoName = config.GetSection("SetTwo:Name").Value;
                string setTwoValue = config["SetTwo:Value"];
                int setTwoAge = config.GetSection("SetTwo:Age").Get<int>();
            }

            // 没有设置的配置，不会报错。在 get<int> 时，也会转为默认值0
            var notset = config.GetSection("NotSetting");
            int notset_int = config.GetSection("NotSetting").Get<int>();

            string setThreeStr = config.GetSection("SetThree").Value;
            string setFourStr = config["SetFour"];

            // memoryConfig
            string myString = config["myString"];
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "welcome to ConfigController";
        }
    }
}