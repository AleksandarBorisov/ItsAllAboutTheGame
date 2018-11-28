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


        public async Task<CreditCard> AddCard(string cardNumber,
                string lastDigits, string cvv, DateTime expiryDate, ClaimsPrincipal userClaims)
        {
            var userId = userManager.GetUserId(userClaims);
            var user = await userManager.FindByIdAsync(userId);

            var creditCard = new CreditCard
            {
                CardNumber = cardNumber,
                LastDigits = lastDigits,
                CVV = cvv,
                ExpiryDate = expiryDate,
                UserId = userId,
                User = user,              
                CreatedOn = DateTime.Now
            };


            this.context.CreditCards.Add(creditCard);
            await this.context.SaveChangesAsync();

            return creditCard;
        }
    }
}
