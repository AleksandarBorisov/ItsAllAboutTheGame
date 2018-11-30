using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Models.AccountViewModels;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.ViewComponents
{
    public class LoginStatusViewComponent : ViewComponent
    {
        private readonly SignInManager<User> signInManager;
        private readonly IUserService userService;
        private readonly IMemoryCache cache;

        public LoginStatusViewComponent(IUserService userService, IMemoryCache cache, SignInManager<User> signInManager)
        {
            this.cache = cache;
            this.userService = userService;
            this.signInManager = signInManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = HttpContext.User;

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
