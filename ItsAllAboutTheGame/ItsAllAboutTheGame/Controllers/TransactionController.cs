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
        public async Task<IActionResult> Deposit(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var claims = HttpContext.User;
            var user = await userManager.GetUserAsync(claims);
            var userCards = await this.cardService.GetSelectListCards(user);

            var userWallet = await this.walletService.GetUserWallet(user);

            var cardCurrency = userWallet.Currency;

            var model = new NewDepositViewModel();
            model.CardCurrency = cardCurrency;
            model.Cards = userCards.ToList();


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(NewDepositViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Deposit", "Transaction");
            }

            var claims = HttpContext.User;
            var user = await userManager.GetUserAsync(claims);
            var userCards = await this.cardService.GetSelectListCards(user);
            var userWallet = await this.walletService.GetUserWallet(user);


            var userCard = await this.cardService.GetCard(user, model.CreditCardId);

            model.Cards = userCards.ToList();
            var cardCurrency = userWallet.Currency;
            model.CardCurrency = cardCurrency;

            model.Cards = userCards.ToList();

            var deposit = await this.transactionService.MakeDeposit(userCard, claims, model.Amount);

            TempData["Success"] = $"Deposit of {model.Amount} made successfully!";
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
                var cardToAdd = await this.cardService.AddCard(model.CardNumber, model.CVV, model.ExpiryDate, user);
                var userWallet = await this.walletService.GetUserWallet(user);
                var userCurrency = userWallet.Currency;

                model.CardNumber = new string('X', cardToAdd.CardNumber.Length - 4) + cardToAdd.CardNumber.Substring(cardToAdd.CardNumber.Length - 4);
                model.CVV = cardToAdd.CVV;
                model.ExpiryDate = cardToAdd.ExpiryDate;
                return RedirectToAction("AddCard","Transaction");
            }


            return this.View(model);
        }




        [AcceptVerbs("Get", "Post")]
        public IActionResult DoesExist(string CardNumber)
        {
            try
            {
                return Json(DoesCardExist(CardNumber));
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        private bool DoesCardExist(string cardNumber)
        {
            var cards = this.context.CreditCards.ToList();

            if (cards.Any(k => k.CardNumber == cardNumber))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult AreDigits(string CVV)
        {
            try
            {
                return Json(AreOnlyDigits(CVV));
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        private bool AreOnlyDigits(string cvv)
        {
            int number;

            bool result = int.TryParse(cvv, out number);

            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
