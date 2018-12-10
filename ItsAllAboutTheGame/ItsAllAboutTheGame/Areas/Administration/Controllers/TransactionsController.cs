using ItsAllAboutTheGame.Areas.Administration.Models;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            var transactions = this.transactionService.GetAllTransactions();

            var model = new TransactionsViewModel(transactions);

            model.SortOrder = model.SortOrder ?? GlobalConstants.DefultTransactionSorting;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateTable(TransactionsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.Clear();

                var oldTransactions = this.transactionService.GetAllTransactions();

                var oldModel = new TransactionsViewModel(oldTransactions);

                oldModel.SortOrder = oldModel.SortOrder ?? GlobalConstants.DefultTransactionSorting;

                return PartialView("_UsersTablePartial", oldModel);
            }

            var transactions = this.transactionService.GetAllTransactions(model.SearchString, model.PageNumber, model.PageSize, model.SortOrder);

            var newModel = new TransactionsViewModel(transactions);

            newModel.SearchString = model.SearchString;

            newModel.SortOrder = model.SortOrder;

            return PartialView("_UsersTablePartial", newModel);
        }
    }
}
