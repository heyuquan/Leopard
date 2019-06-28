using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Leopard.Template.MVC.Models;
using Leopard.AspNetCore.Extensions;

namespace Leopard.Template.MVC.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            this.RecordInSession("Index");
            return View();
        }

        public IActionResult Privacy()
        {
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
