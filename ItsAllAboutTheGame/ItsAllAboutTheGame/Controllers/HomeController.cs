using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ItsAllAboutTheGame.Models;

namespace ItsAllAboutTheGame.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            
        }

        [HttpGet]
        public IActionResult Index()
        {
            this.ViewData["ReturnUrl"] = "/Home/Index";
            return View();
        }

        [HttpGet]
        public IActionResult ResponsibleGambling()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
