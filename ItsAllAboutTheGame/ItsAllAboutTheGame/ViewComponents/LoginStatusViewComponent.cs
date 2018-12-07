using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Models.AccountViewModels;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.ViewComponents
{
    public class LoginStatusViewComponent : ViewComponent
    {
        private readonly SignInManager<User> signInManager;
        private readonly IUserService userService;

        public LoginStatusViewComponent(IUserService userService, SignInManager<User> signInManager)
        {
            this.userService = userService;
            this.signInManager = signInManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (signInManager.IsSignedIn(HttpContext.User))
            {
                var userInfo = await this.userService.GetUserInfo(HttpContext.User);

                var info = new UserInfoViewModel(userInfo);

                return View("LoggedIn", info);
            }
            return View();
        }
    }
}
