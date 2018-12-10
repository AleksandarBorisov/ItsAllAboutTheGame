using ItsAllAboutTheGame.Areas.Administration.Models;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Areas.Administration.Controllers
{
    [Area(GlobalConstants.AdminArea)]
    [Authorize(Roles = GlobalConstants.AdminRole + "," + GlobalConstants.MasterAdminRole)]
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

            model.SortOrder = model.SortOrder ?? GlobalConstants.DefultUserSorting;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateTable(UsersViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.Clear();

                var oldUsers = this.userService.GetAllUsers();

                var oldModel = new UsersViewModel(oldUsers);

                oldModel.SortOrder = oldModel.SortOrder ?? GlobalConstants.DefultUserSorting;

                return PartialView("_UsersTablePartial", oldModel);
            }

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
