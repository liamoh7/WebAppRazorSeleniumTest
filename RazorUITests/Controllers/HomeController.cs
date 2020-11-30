using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RazorUITests.Models;
using RazorUITests.Services;

namespace RazorUITests.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IHomeService homeService)
        {
            this.HomeService = homeService;
        }
        
        private IHomeService HomeService { get; }

        public IActionResult Index()
        {
            var str = this.HomeService.GoHome();
            this.ViewData.Add("home-type", str);
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}