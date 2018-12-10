using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Enums;
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
        public async Task<IActionResult> GameOne(string gridSize)
        {
            if (!GlobalConstants.GameGrids.Contains(gridSize))
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await userManager.GetUserAsync(HttpContext.User);

            var userWallet = await this.walletService.GetUserWallet(user);

            var gameGrid = gameOne.GenerateGrid(gridSize);

            var model = new WalletViewModel(userWallet, gameGrid, gridSize);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GameOneSpin(WalletViewModel model)
        {
            //If we pass this If the Grid is valid
            if (!GlobalConstants.GameGrids.Contains(model.GridSize))
            {
                return RedirectToAction("Index","Home");
            }

            var user = await userManager.GetUserAsync(HttpContext.User);

            var oldWallet = await this.walletService.GetUserWallet(user);

            if (!ModelState.IsValid || !(Math.Abs(Math.Round(oldWallet.Balance, 2) - Math.Round(model.Balance, 2)) < 0.02m))
            {
                ModelState.Clear();

                var gameGrid = gameOne.GenerateGrid(model.GridSize);

                var oldModel = new WalletViewModel(oldWallet, gameGrid, model.GridSize);

                return PartialView("_GameOnePartial", oldModel);
            }

            var userWallet = await this.walletService.UpdateUserWallet(user, -(decimal)model.Stake);

            await this.transactionService.GameTransaction(user, (int)model.Stake, model.GridSize, GlobalConstants.StakeDescription, TransactionType.Stake);

            var gameResult = gameOne.Play((int)model.Stake, model.GridSize);

            if (gameResult.WonAmount != 0.0m)
            {
                await this.transactionService.GameTransaction(user, (int)gameResult.WonAmount, model.GridSize, GlobalConstants.WinDescription, TransactionType.Win);

                userWallet = await this.walletService.UpdateUserWallet(user, gameResult.WonAmount);
            }

            var newModel = new WalletViewModel(userWallet, gameResult, model.GridSize);

            return PartialView("_GameOnePartial", newModel);
        }
    }
}