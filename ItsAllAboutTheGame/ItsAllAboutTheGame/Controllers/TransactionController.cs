using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Models.TransactionViewModels;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class TransactionController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly ItsAllAboutTheGameDbContext context;
        private readonly ICardService cardService; 

        public TransactionController(UserManager<User> userManager, ItsAllAboutTheGameDbContext context,
            ICardService cardService)
        {
            this.context = context;
            this.userManager = userManager;
            this.cardService = cardService;
        }


        [HttpGet]
        public async Task<IActionResult> Deposit(NewDepositViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var claims = HttpContext.User;
            var user = await userManager.GetUserAsync(claims);
            var userCards = this.context.CreditCards.Where(c => c.User == user)
                .Select(c => new SelectListItem { Value = c.Id.ToString(),
                    Text = new string('*', c.CardNumber.Length - 4) + c.CardNumber.Substring(c.CardNumber.Length - 4)}).ToList();

            

            //var cardCurrency = userWallet.Currency;
           // model.CardCurrency = cardCurrency;
            model.Cards = userCards;


            return View(model);
        }

        //[HttpPost]
        //public async Task<IActionResult> Deposit(NewDepositViewModel model, string returnUrl = null)
        //{
        //    ViewData["ReturnUrl"] = returnUrl;

        //    if (ModelState.IsValid)
        //    {
        //        var claims = HttpContext.User;
        //        var user = await userManager.GetUserAsync(claims);


        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> AddCard(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCard(AddCardViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var userName = HttpContext.User.Identity.Name;
                var user = await this.userManager.FindByNameAsync(userName);
                var cardToAdd = await this.cardService.AddCard(model.CardNumber, model.CVV, model.ExpiryDate,  user);
                var userCurrency = user.Wallet.Currency;

                model.CardNumber = new string('X', cardToAdd.CardNumber.Length - 4) + cardToAdd.CardNumber.Substring(cardToAdd.CardNumber.Length - 4);
                model.CVV = cardToAdd.CVV;
                model.ExpiryDate = cardToAdd.ExpiryDate;
                return this.RedirectToAction("Deposit", "Transaction");
            }
            

            return this.View(model);
        }
    }
}
