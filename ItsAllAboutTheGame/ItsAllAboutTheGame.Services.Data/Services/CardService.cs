using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.Data.Services
{
    public class CardService : ICardService
    {
        private readonly ItsAllAboutTheGameDbContext context;
        private readonly UserManager<User> userManager;

        public CardService(ItsAllAboutTheGameDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }


        //public async Task<CreditCard> AddCard(string cardName, string paymentToken, ClaimsPrincipal userClaims)
        //{
        //    var userId = userManager.GetUserId(userClaims);
        //    var user =  await userManager.FindByIdAsync(userId);

        //    var creditCard = new CreditCard
        //    {
        //        UserId = userId,
        //        User = user,
        //        PaymentToken = paymentToken,
        //        CardName = cardName,
        //        CreatedOn = DateTime.Now
        //    };

        //    this.context.CreditCards.Add(creditCard);
            
        //    await this.context.SaveChangesAsync();
        //    //this.context.Update(user); ?! Should I or should I not?
        //    return creditCard;
        //}
    }
}
