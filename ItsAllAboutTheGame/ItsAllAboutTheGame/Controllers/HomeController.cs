using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ItsAllAboutTheGame.Models;
using ItsAllAboutTheGame.Services.Data.Contracts;

namespace ItsAllAboutTheGame.Controllers
{
    public class HomeController : Controller
    {
        private IForeignExchangeService foreignExchangeService;

        public HomeController(IForeignExchangeService foreignExchangeService)
        {
            this.foreignExchangeService = foreignExchangeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currencies = await foreignExchangeService.GetConvertionRates();

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
