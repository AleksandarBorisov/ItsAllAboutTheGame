using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            map = CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.IsNeutralCulture)
                .Select(culture =>
                {
                    try
                    {
                        return new RegionInfo(culture.LCID);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(ri => ri != null)
                .GroupBy(ri => ri.ISOCurrencySymbol)
                .ToDictionary(x => x.Key, x => x.First().CurrencySymbol);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (singInManager.IsSignedIn(HttpContext.User))
            {
                var userId = userManager.GetUserId(HttpContext.User);
                var user = await userManager.Users.Where(x => x.Id.Equals(userId))
                    .Include(player => player.Wallet)
                    .FirstOrDefaultAsync();
                var info = new UserInfoViewModel(user, TryGetCurrencySymbol(user.Wallet.Currency.ToString(), out string currSymbol) ? currSymbol : "");
                return View("LoggedIn", info);
            }
            else
            {
                return View();
            }
        }

        private static IDictionary<string, string> map;

        //static CurrencyTools()
        //{
            
        //}

        public static bool TryGetCurrencySymbol(string ISOCurrencySymbol, out string symbol)
        {
            return map.TryGetValue(ISOCurrencySymbol, out symbol);
        }
    }
}
