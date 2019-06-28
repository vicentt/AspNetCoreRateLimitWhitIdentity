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
        private readonly ApiClientService ApiClientService;

        public HomeController(ApiClientService apiClientService)
        {
            ApiClientService = apiClientService;
        }

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
            var response = await ApiClientService.CallApi();
            ViewBag.Response = $"{(int)response.StatusCode} -- {response.StatusCode}";
            return View("CallApi");
        }

        public async Task<IActionResult> CallApiUnAuthorized()
        {
            var HttpClient = new HttpClient();
            var response = await HttpClient.GetAsync("http://localhost:51031/api/values");
            ViewBag.Response = $"{(int)response.StatusCode} -- {response.StatusCode}";
            return View("CallApi");
        }
    }
}
