using ItsAllAboutTheGame.Areas.Administration.Models;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Areas.Administration.Controllers
{
    [Area(GlobalConstants.AdminArea)]
    [Authorize(Roles = GlobalConstants.AdminRole + "," + GlobalConstants.MasterAdminRole)]
    public class TransactionsController : Controller
    {
        private readonly ITransactionService transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var transactions = await this.transactionService.GetAllTransactions();

            var model = new TransactionsViewModel(transactions);

            ViewData["BaseCurrencySymbol"] = GlobalConstants.BaseCurrencySymbol;

            model.SortOrder = model.SortOrder ?? GlobalConstants.DefultTransactionSorting;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTable(TransactionsViewModel model)
        {
            ViewData["BaseCurrencySymbol"] = GlobalConstants.BaseCurrencySymbol;

            if (!ModelState.IsValid)
            {
                ModelState.Clear();

                var oldTransactions = await this.transactionService.GetAllTransactions();

                var oldModel = new TransactionsViewModel(oldTransactions);

                oldModel.SortOrder = oldModel.SortOrder ?? GlobalConstants.DefultTransactionSorting;

                return PartialView("_TransactionsTablePartial", oldModel);
            }

            var transactions = await this.transactionService.GetAllTransactions(model.SearchString, model.PageNumber, model.PageSize, model.SortOrder);

            var newModel = new TransactionsViewModel(transactions);

            newModel.SearchString = model.SearchString;

            newModel.SortOrder = model.SortOrder;

            return PartialView("_TransactionsTablePartial", newModel);
        }
    }
}
