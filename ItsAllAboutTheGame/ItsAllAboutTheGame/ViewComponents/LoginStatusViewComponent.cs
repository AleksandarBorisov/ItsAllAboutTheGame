using ItsAllAboutTheGame.Models.AccountViewModels;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.ViewComponents
{
    public class LoginStatusViewComponent : ViewComponent
    {
        private readonly IUserService userService;
        private readonly IMemoryCache cache;

        public LoginStatusViewComponent(IUserService userService, IMemoryCache cache)
        {
            this.cache = cache;
            this.userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userInfo = await this.userService.GetUserInfo(HttpContext.User);

            if (userInfo.IsSignedIn)
            {
                var info = new UserInfoViewModel(userInfo);
                return View("LoggedIn", info);
            }
            else
            {
                return View();
            }
        }
    }
}
