using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.Models.TransactionViewModels;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IForeignExchangeService foreignExchangeService;
        private readonly IDateTimeProvider dateTimeProvider;

        public TransactionController(UserManager<User> userManager, ItsAllAboutTheGameDbContext context,
            ICardService cardService, IWalletService walletService, ITransactionService transactionService,
            IForeignExchangeService foreignExchangeService, IDateTimeProvider dateTimeProvider)
        {
            this.context = context;
            this.userManager = userManager;
            this.cardService = cardService;
            this.walletService = walletService;
            this.dateTimeProvider = dateTimeProvider;
            this.transactionService = transactionService;
            this.foreignExchangeService = foreignExchangeService;
        }

        [HttpGet]
        public async Task<IActionResult> Deposit(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var user = await userManager.GetUserAsync(HttpContext.User);

            var userCards = await this.cardService.GetSelectListCards(user);

            var userCardsForDelete = await this.cardService.GetSelectListCards(user);

            var userWallet = await this.walletService.GetUserWallet(user);

            var model = new NewDepositViewModel();

            model.CardCurrencySymbol = userWallet.CurrencySymbol;
            model.Cards = userCards.ToList();
            model.CardsForDelete = userCardsForDelete.ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(NewDepositViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Deposit", "Transaction");
            }

            var user = await userManager.GetUserAsync(HttpContext.User);

            var userCards = await this.cardService.GetSelectListCards(user);

            var userDeposit = await this.transactionService.MakeDeposit(user, (int)model.CreditCardId, (int)model.Amount);

            var convertedAmount = await this.walletService.ConvertBalance(user);

            return Json(new { Balance = convertedAmount });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(NewDepositViewModel model)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            var oldWallet = await this.walletService.GetUserWallet(user);

            if (!ModelState.IsValid || (Math.Round(oldWallet.Balance, 2) < Math.Round((decimal)model.Amount, 2)))
            {
                return RedirectToAction("Deposit", "Transaction");
            }

            var withdrawedAmount = await this.walletService.WithdrawFromUserBalance(user, (int)model.Amount, (int)model.CreditCardId);

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCard(AddCardViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var userName = HttpContext.User.Identity.Name;

                var user = await this.userManager.FindByNameAsync(userName);

                var dateStrings = model.ExpiryDate.Split('/');

                var expiryDate = $"01.{dateStrings[0]}.20{dateStrings[1]}";

                var cardToAdd = await this.cardService.AddCard(model.CardNumber.Replace(" ",""), model.CVV, DateTime.Parse(expiryDate), user);

                return RedirectToAction("Deposit", "Transaction");
            }

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCard(int? cardId)
        {
            if (cardId == null)
            {
                return RedirectToAction(nameof(TransactionController.Deposit));
            }

            var user = await userManager.GetUserAsync(HttpContext.User);

            var deletedCard = await this.cardService.DeleteCard((int)cardId);

            var userCardsForDelete = await this.cardService.GetSelectListCards(user);

            return Json(userCardsForDelete);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            var userCardsForDelete = await this.cardService.GetSelectListCards(user, false);

            return Json(userCardsForDelete);
        }

        // Methods for remote attributes!
        // LOOK DOWN
        [AcceptVerbs("Get", "Post")]
        public IActionResult DoesExist(string CardNumber)
        {
            try
            {
                return Json(this.cardService.DoesCardExist(CardNumber.Replace(" ", "")));
                // if returned boolean is false the card exists
                // if returned boolean is true the card does NOT exist so the card number is valid
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsDateValid(string ExpiryDate)
        {
            var dateStrings = ExpiryDate.Split('/');

            if (dateStrings.Length < 2 || !int.TryParse(dateStrings[0], out int month) || !int.TryParse(dateStrings[0], out int year))
            {
                return Json(false);
            }

            var isValidDate = DateTime.TryParseExact($"01.{dateStrings[0]}.20{dateStrings[1]}",
                       "dd.MM.yyyy",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out DateTime expiryDateAsDate);

            var now = dateTimeProvider.Now;

            if (!isValidDate)
            {
                return Json(false);
            }
            else
            {
                if (now < expiryDateAsDate)
                {
                    return Json(true);
                }

                return Json(false);
            }
        }
    }
}
