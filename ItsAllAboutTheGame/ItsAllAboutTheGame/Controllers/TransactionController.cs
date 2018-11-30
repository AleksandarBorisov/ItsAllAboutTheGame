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
        private readonly IWalletService walletService;
        private readonly ITransactionService transactionService;

        public TransactionController(UserManager<User> userManager, ItsAllAboutTheGameDbContext context,
            ICardService cardService, IWalletService walletService, ITransactionService transactionService)
        {
            this.context = context;
            this.userManager = userManager;
            this.cardService = cardService;
            this.walletService = walletService;
            this.transactionService = transactionService;
        }


        [HttpGet]
        public async Task<IActionResult> Deposit(NewDepositViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var claims = HttpContext.User;
            var user = await userManager.GetUserAsync(claims);
            var userCards = await this.cardService.GetSelectListCards(user);

            var userWallet = await this.walletService.GetUserWallet(user);

            var cardCurrency = userWallet.Currency;
            model.CardCurrency = cardCurrency;
            model.Cards = userCards.ToList();
          

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(NewDepositViewModel model)
        {
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
                var claims = HttpContext.User;
                var user = await userManager.GetUserAsync(claims);
                var userCards = await this.cardService.GetSelectListCards(user);
                var userWallet = await this.walletService.GetUserWallet(user);


                var userCard = await this.cardService.GetCard(user, model.CreditCardId);

                //model.Cards = userCards;
                var cardCurrency = userWallet.Currency;
                model.CardCurrency = cardCurrency;

                model.Cards = userCards.ToList();

                var deposit = await this.transactionService.MakeDeposit(userCard, claims, model.Amount);

                return RedirectToAction("Index", "Home");
                                                               
            
        }

        [HttpGet]
        public IActionResult AddCard(string returnUrl = null)
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
                var userWallet = await this.walletService.GetUserWallet(user);
                var userCurrency = userWallet.Currency;

                model.CardNumber = new string('X', cardToAdd.CardNumber.Length - 4) + cardToAdd.CardNumber.Substring(cardToAdd.CardNumber.Length - 4);
                model.CVV = cardToAdd.CVV;
                model.ExpiryDate = cardToAdd.ExpiryDate;
                return this.RedirectToAction("Deposit", "Transaction");
            }
            

            return this.View(model);
        }
    }
}
