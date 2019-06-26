using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Client.Models;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using IdentityModel.Client;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> CallApi()
        {
            DiscoveryClient discoveryClient = new DiscoveryClient("http://localhost:5000/");

            // Accept the configuration even if the issuer and endpoints don't match
            discoveryClient.Policy.ValidateIssuerName = false;
            discoveryClient.Policy.ValidateEndpoints = false;
            var discoResponse = await discoveryClient.GetAsync();

            var client = new HttpClient();
            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoResponse.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            });

            var clientHttp = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.AccessToken);
            var content = await client.GetStringAsync("http://localhost:51031/api/values");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("CallApi");
        }
    }
}
