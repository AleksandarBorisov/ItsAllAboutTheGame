using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItsAllAboutTheGame.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class TransactionController : Controller
    {
        public TransactionController()
        {

        }


        [HttpGet]
        public IActionResult Deposit(string returnUrl = null)
        {

            return View();
        }
    }
}
