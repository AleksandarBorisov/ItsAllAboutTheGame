using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Models.TransactionViewModels;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly IForeignExchangeService foreignExchangeService;

        public TransactionController(UserManager<User> userManager, ItsAllAboutTheGameDbContext context,
            ICardService cardService, IWalletService walletService, ITransactionService transactionService,
            IForeignExchangeService foreignExchangeService)
        {
            this.context = context;
            this.userManager = userManager;
            this.cardService = cardService;
            this.walletService = walletService;
            this.transactionService = transactionService;
            this.foreignExchangeService = foreignExchangeService;
        }


        [HttpGet]
        public async Task<IActionResult> Deposit(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var claims = HttpContext.User;
            var user = await userManager.GetUserAsync(claims);
            var userId = await userManager.GetUserIdAsync(user);
            var userCards = await this.cardService.GetSelectListCards(user);
            var userCardsForDelete = await this.cardService.GetSelectListCardsForDelete(user);

            var userWallet = await this.walletService.GetUserWallet(user);

            var model = new NewDepositViewModel();

            model.CardCurrency = userWallet.Currency;
            model.Cards = userCards.ToList();
            model.CardsForDelete = userCardsForDelete.ToList();

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

            var deposit = await this.transactionService.MakeDeposit(user, model.CreditCardId, model.Amount);


            var convertedAmount = await this.walletService.ConvertBalance(user);


            return Json(new { Balance = convertedAmount });
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

                return RedirectToAction("Deposit", "Transaction");
            }

            return this.View();
        }


        [HttpPost]
        public async Task<IActionResult> DeleteCard(NewDepositViewModel model)
        {
            var claims = HttpContext.User;
            var user = await userManager.GetUserAsync(claims);
            var userId = await userManager.GetUserIdAsync(user);
            var userCardsForDelete = await this.cardService.GetSelectListCardsForDelete(user);
            
            var cardToDelete = await this.cardService.DeleteCard(userId, model.CreditCardId);

            return RedirectToAction("Deposit", "Transaction");
        }


        // Methods for remote attributes!
        // LOOK DOWN


        [AcceptVerbs("Get", "Post")]
        public IActionResult DoesExist(string CardNumber)
        {
            try
            {
                return Json(this.cardService.DoesCardExist(CardNumber));
                // if returned boolean is false the card exists
                // if returned boolean is true the card does NOT exist so the card number is valid
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult AreDigits(string CVV)
        {
            try
            {
                return Json(this.cardService.AreOnlyDigits(CVV));
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsDateValid(DateTime ExpiryDate)
        {
            try
            {
                return Json(this.cardService.IsExpired(ExpiryDate));
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
    }
}
