using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Leopard.Template.MVC.Models;
using Leopard.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using CookieManager;

namespace Leopard.Template.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICookieManager _CookieManager;
        private readonly ICookie _Cookie;

        public HomeController(IHttpContextAccessor httpContextAccessor
            , ICookieManager CookieManager
            , ICookie Cookie
            )
        {
            this._httpContextAccessor = httpContextAccessor;
            this._CookieManager = CookieManager;
            this._Cookie = Cookie;
        }

        public IActionResult Index()
        {
            this.RecordInSession("Index");

            _httpContextAccessor.HttpContext.Response.Cookies.Append("key", "key_hyq");

            Response.Cookies.Append("key2", "key2_hyq");

            _CookieManager.Set("_CookieManager_key", "_CookieManager_hyq");

            _Cookie.Set("_Cookie_key", "_Cookie_hyq", 10);
            return View();
        }

        public IActionResult Privacy()
        {
            //read cookie from IHttpContextAccessor  
            string cookieValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies["key"];

            //read cookie from Request object  
            string cookieValueFromReq = Request.Cookies["Key2"];

            string cookieValueFromCookieManager = _CookieManager.Get<string>("_CookieManager_key");

            string cookieValueFromCookie = _Cookie.Get("_Cookie_key");
            this.RecordInSession("Privacy");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void RecordInSession(string action)
        {
            var paths = HttpContext.Session.GetObject<string>("actions") ?? string.Empty;
            HttpContext.Session.SetObject("actions", paths + ";" + action);
        }
    }
}
