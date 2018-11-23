using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ItsAllAboutTheGame.Models;
using ItsAllAboutTheGame.Services.External.Contracts;

namespace ItsAllAboutTheGame.Controllers
{
    public class HomeController : Controller
    {
        private IForeignExchangeApiCaller apiCaller;

        public HomeController(IForeignExchangeApiCaller apiCaller)
        {
            this.apiCaller = apiCaller;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var currencies = await apiCaller.GetRates();

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
