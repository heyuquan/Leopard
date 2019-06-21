using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Leopard.Template.WebAPI.HttpClients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leopard.Template.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HttpClientController : ControllerBase
    {
        public IHttpClientFactory ClientFactory { get; set; }

        public HttpClient httpClient { get; set; }

        public GitHubService GitHubService { get; set; }

        [HttpGet]
        public ActionResult<string> Get()
        {
            var client_from_factory = ClientFactory.CreateClient();

            var client_from_name_factory = ClientFactory.CreateClient("github");

            var client_by_type = GitHubService.Client;

            return "welcome to HttpClientController";
        }
    }
}