using ItsAllAboutTheGame.Data.Constants;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItsAllAboutTheGame.Areas.Administration.Controllers
{
    //[Area(DataConstants.AdminArea)]
    [Authorize(Roles = DataConstants.MasterAdminRole)]
    public class UsersController : Controller
    {
        private IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }


        public IActionResult Index()
        {
            //var users = this.userService.GetAllUsers();

            return View();
        }
    }
}
