using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Models.GameViewModels;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Game.Contracts.GameOne;
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
        private readonly IGameOne gameOne;

        public GameController(IWalletService walletService,
            UserManager<User> userManager,
            ITransactionService transactionService,
            IGameOne gameOne)
        {
            this.transactionService = transactionService;
            this.walletService = walletService;
            this.userManager = userManager;
            this.gameOne = gameOne;
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

            var gameGrid = gameOne.GenerateGrid();

            var model = new WalletViewModel(userWallet, gameGrid);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GameOneSpin(WalletViewModel model)
        {//TODO: Test the view model for int? behaviour
            var user = await userManager.GetUserAsync(HttpContext.User);

            var oldWallet = await this.walletService.GetUserWallet(user);

            if (!ModelState.IsValid || !(Math.Abs(Math.Round(oldWallet.Balance, 2) - Math.Round(model.Balance, 2)) < 0.02m))
            {
                ModelState.Clear();

                var gameGrid = gameOne.GenerateGrid();

                var oldModel = new WalletViewModel(oldWallet, gameGrid);

                return PartialView("_GameOnePartial", oldModel);
            }

            ModelState.Remove("Stake");

            var userWallet = await this.walletService.UpdateUserWallet(user, -(decimal)model.Stake);

            await this.transactionService.MakeStake(user, (int)model.Stake, "3x4");

            var gameResult = gameOne.Play((int)model.Stake);

            if (gameResult.WonAmount != 0.0m)
            {
                userWallet = await this.walletService.UpdateUserWallet(user, gameResult.WonAmount);
            }

            var newModel = new WalletViewModel(userWallet, gameResult);

            return PartialView("_GameOnePartial", newModel);
        }
    }
}