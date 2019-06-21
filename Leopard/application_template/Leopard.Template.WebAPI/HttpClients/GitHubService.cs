using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace Leopard.Template.WebAPI.HttpClients
{
    public class GitHubService
    {
        public HttpClient Client { get; }

        public GitHubService(HttpClient client)
        {
            client.BaseAddress = new Uri("https://api.github.com/");
            // Github API versioning
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");

            Client = client;
        }
    }
}
