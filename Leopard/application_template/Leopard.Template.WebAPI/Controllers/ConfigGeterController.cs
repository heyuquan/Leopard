using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Leopard.Configuration;
using Leopard.Template.WebAPI.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leopard.Template.WebAPI.Controllers
{
    [Route("api/config/geter")]
    [ApiController]
    public class ConfigGeterController : ControllerBase
    {
        private readonly IConfigurationGeter configurationGeter;
        private readonly MySettingDI mySetting;

        public ConfigGeterController(MySettingDI mySetting, IConfigurationGeter configurationGeter)
        {
            this.configurationGeter = configurationGeter;
            this.mySetting = mySetting;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            var data = new
            {
                Action= "welcome to ConfigGeterController",
                TestConfigStarshipName = configurationGeter["MySettingDI:SetOneDI:Value"],
                FromClass = this.mySetting,
                FromGetter = configurationGeter.Get<MySettingDI>(),
                FromLocator = ConfigurationGeterLocator.Current.Get<MySettingDI>()
            };
            return new JsonResult(data);
        }
    }
}