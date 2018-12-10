using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
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

            var gameGrid = gameOne.GenerateGrid(GlobalConstants.GameOneGrid);

            var model = new WalletViewModel(userWallet, gameGrid);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GameOneSpin(WalletViewModel model)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            var oldWallet = await this.walletService.GetUserWallet(user);

            if (!ModelState.IsValid || !(Math.Abs(Math.Round(oldWallet.Balance, 2) - Math.Round(model.Balance, 2)) < 0.02m))
            {
                ModelState.Clear();

                var gameGrid = gameOne.GenerateGrid(GlobalConstants.GameOneGrid);

                var oldModel = new WalletViewModel(oldWallet, gameGrid);

                return PartialView("_GameOnePartial", oldModel);
            }

            ModelState.Remove("Stake");

            var userWallet = await this.walletService.UpdateUserWallet(user, -(decimal)model.Stake);

            await this.transactionService.MakeStake(user, (int)model.Stake, GlobalConstants.GameOneGrid);

            var gameResult = gameOne.Play((int)model.Stake, GlobalConstants.GameOneGrid);

            if (gameResult.WonAmount != 0.0m)
            {
                userWallet = await this.walletService.UpdateUserWallet(user, gameResult.WonAmount);
            }

            var newModel = new WalletViewModel(userWallet, gameResult);

            return PartialView("_GameOnePartial", newModel);
        }

        [HttpGet]
        public async Task<IActionResult> GameTwo()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            var userWallet = await this.walletService.GetUserWallet(user);

            var gameGrid = gameOne.GenerateGrid(GlobalConstants.GameTwoGrid);

            var model = new WalletViewModel(userWallet, gameGrid);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GameTwoSpin(WalletViewModel model)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            var oldWallet = await this.walletService.GetUserWallet(user);

            if (!ModelState.IsValid || !(Math.Abs(Math.Round(oldWallet.Balance, 2) - Math.Round(model.Balance, 2)) < 0.02m))
            {
                ModelState.Clear();

                var gameGrid = gameOne.GenerateGrid(GlobalConstants.GameTwoGrid);

                var oldModel = new WalletViewModel(oldWallet, gameGrid);

                return PartialView("_GameTwoPartial", oldModel);
            }

            ModelState.Remove("Stake");

            var userWallet = await this.walletService.UpdateUserWallet(user, -(decimal)model.Stake);

            await this.transactionService.MakeStake(user, (int)model.Stake, GlobalConstants.GameTwoGrid);

            var gameResult = gameOne.Play((int)model.Stake, GlobalConstants.GameTwoGrid);

            if (gameResult.WonAmount != 0.0m)
            {
                userWallet = await this.walletService.UpdateUserWallet(user, gameResult.WonAmount);
            }

            var newModel = new WalletViewModel(userWallet, gameResult);

            return PartialView("_GameTwoPartial", newModel);
        }

        [HttpGet]
        public async Task<IActionResult> GameThree()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            var userWallet = await this.walletService.GetUserWallet(user);

            var gameGrid = gameOne.GenerateGrid(GlobalConstants.GameThreeGrid);

            var model = new WalletViewModel(userWallet, gameGrid);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GameThreeSpin(WalletViewModel model)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            var oldWallet = await this.walletService.GetUserWallet(user);

            if (!ModelState.IsValid || !(Math.Abs(Math.Round(oldWallet.Balance, 2) - Math.Round(model.Balance, 2)) < 0.02m))
            {
                ModelState.Clear();

                var gameGrid = gameOne.GenerateGrid(GlobalConstants.GameThreeGrid);

                var oldModel = new WalletViewModel(oldWallet, gameGrid);

                return PartialView("_GameThreePartial", oldModel);
            }

            ModelState.Remove("Stake");

            var userWallet = await this.walletService.UpdateUserWallet(user, -(decimal)model.Stake);

            await this.transactionService.MakeStake(user, (int)model.Stake, GlobalConstants.GameThreeGrid);

            var gameResult = gameOne.Play((int)model.Stake, GlobalConstants.GameThreeGrid);

            if (gameResult.WonAmount != 0.0m)
            {
                userWallet = await this.walletService.UpdateUserWallet(user, gameResult.WonAmount);
            }

            var newModel = new WalletViewModel(userWallet, gameResult);

            return PartialView("_GameThreePartial", newModel);
        }
    }
}