using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Enums;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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


        public async Task<CreditCard> AddCard(string cardNumber, string cvv, DateTime expiryDate, User user)
        {
            var userId = user.Id;

            var creditCard = new CreditCard
            {
                CardNumber = cardNumber,
                LastDigits = cardNumber.Substring(cardNumber.Length - 4),
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

        public async Task<CreditCard> GetCard(User user)
        {
            var userCard = await this.context.CreditCards.Where(c => c.User == user).FirstOrDefaultAsync();

            return userCard;
        }

        public async Task<IEnumerable<SelectListItem>> GetSelectListCards(User user)
        {
            var userCards = await this.context.CreditCards.Where(c => c.User == user)
               .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = new string('*', c.CardNumber.Length - 4) + c.CardNumber.Substring(c.CardNumber.Length - 4)
                }).ToListAsync();

            return userCards.ToList();
        }
    }
}
