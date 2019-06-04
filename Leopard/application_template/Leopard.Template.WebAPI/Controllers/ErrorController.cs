using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leopard.Template.WebAPI.Controllers
{
    [Route("api/error")]
    [ApiController]
    public class ErrorController
    {
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            throw new Exception("模拟异常");
            return "value1";
        }
    }
}
