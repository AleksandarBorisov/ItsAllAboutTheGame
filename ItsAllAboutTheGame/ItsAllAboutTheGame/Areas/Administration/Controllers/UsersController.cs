using ItsAllAboutTheGame.Areas.Administration.Models;
using ItsAllAboutTheGame.Data.Constants;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Areas.Administration.Controllers
{
    [Area(DataConstants.AdminArea)]
    [Authorize(Roles = "MasterAdministrator")]
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;

        public UsersController(IUserService userService, UserManager<User> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var users = this.userService.GetAllUsers();

            var model = new UsersViewModel(users);

            model.SortOrder = model.SortOrder ?? DataConstants.DefultSorting;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateTable(UsersViewModel model)
        {
            var users = this.userService.GetAllUsers(model.SearchString, model.PageNumber,model.PageSize, model.SortOrder);

            var newModel = new UsersViewModel(users);

            newModel.SearchString = model.SearchString;

            newModel.SortOrder = model.SortOrder;

            return PartialView("_UsersTablePartial", newModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lockout(string userId, int lockoutFor)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var newModel =  await this.userService.LockoutUser(userId, lockoutFor);

            return PartialView("_UserRowPartial", newModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string userId)
        {
            if (userId == null)
            {
                return RedirectToAction("Index");
            }

            var newModel = await this.userService.DeleteUser(userId);

            return PartialView("_UserRowPartial", newModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAdmin(string userId)
        {
            if (userId == null)
            {
                return RedirectToAction("Index");
            }

            var newModel = await this.userService.ToggleAdmin(userId);

            return PartialView("_UserRowPartial", newModel);
        }
    }
}
