using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Models.DepositViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ItsAllAboutTheGame.Controllers
{
    [Authorize]
    public class DepositController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly ItsAllAboutTheGameDbContext context;

        public DepositController(UserManager<User> userManager, ItsAllAboutTheGameDbContext context)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(NewDepositViewModel model)
        {
            var claims = HttpContext.User;
            var user = await userManager.GetUserAsync(claims);
            var userCards = this.context.CreditCards.Where(c => c.User == user)
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.CardNumber}).ToList();

            //var cardCurrency = userCards.Select(c => c.Currency);

            model.Cards = userCards;


            return View(model);
        }
    }
}