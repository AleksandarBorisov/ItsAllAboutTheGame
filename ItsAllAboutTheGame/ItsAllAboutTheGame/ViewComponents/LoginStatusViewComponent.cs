﻿using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.ViewComponents
{
    public class LoginStatusViewComponent : ViewComponent
    {
        private readonly SignInManager<User> singInManager;
        private readonly UserManager<User> userManager;

        public LoginStatusViewComponent(SignInManager<User> singInManager, UserManager<User> userManager)
        {
            this.singInManager = singInManager;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (singInManager.IsSignedIn(HttpContext.User))
            {
                var user = await userManager.GetUserAsync(HttpContext.User);
                var info = new UserInfoViewModel(user);
                return View("LoggedIn", info);
            }
            else
            {
                return View();
            }
        }
    }
}
