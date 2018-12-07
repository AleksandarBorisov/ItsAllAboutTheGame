using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Models.GameViewModels;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly IWalletService walletService;
        private readonly ITransactionService transactionService;
        private readonly UserManager<User> userManager;

        public GameController(IWalletService walletService, UserManager<User> userManager, ITransactionService transactionService)
        {
            this.transactionService = transactionService;
            this.walletService = walletService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GameOne()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            var userWallet = await this.walletService.GetUserWallet(user);

            var model = new WalletViewModel(userWallet);

            return View(model);
        }

        public async Task<IActionResult> GameOneSpin(WalletViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_GameOnePartial", model);
            }

            var user = await userManager.GetUserAsync(HttpContext.User);

            var userWallet = await this.walletService.UpdateUserWallet(user, (int)model.Stake);

            await this.transactionService.MakeStake(user, (int)model.Stake, "3x4");

            var newModel = new WalletViewModel(userWallet);

            return PartialView("_GameOnePartial", newModel);
        }
    }
}