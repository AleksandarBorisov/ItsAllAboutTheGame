using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.Exceptions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.Data.Services
{
    public class CardService : ICardService
    {
        private readonly ItsAllAboutTheGameDbContext context;

        public CardService(ItsAllAboutTheGameDbContext context)
        {
            this.context = context;
        }

        public async Task<CreditCard> AddCard(string cardNumber, string cvv, DateTime expiryDate, User user)
        {
            var userId = user.Id;

            // One card can only be assigned to one user ONLY!!!
            var creditCard = await this.context.CreditCards.Where(cn => cn.CardNumber == cardNumber).FirstOrDefaultAsync();

            if (creditCard != null && creditCard.IsDeleted == true)
            {
                creditCard.ExpiryDate = expiryDate;
                creditCard.CreatedOn = DateTime.Now;
                creditCard.IsDeleted = false;
                this.context.CreditCards.Update(creditCard);
                await this.context.SaveChangesAsync();

                return creditCard;
            }

            else if(creditCard != null && creditCard.IsDeleted == false)
            {
                throw new EntityAlreadyExistsException("Card already exists in the system!");
            }

            creditCard = new CreditCard
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

        public async Task<CreditCard> DeleteCard(int cardId)
        {
            try
            {
                var userCardToDelete = await this.context.CreditCards.FindAsync(cardId);

                userCardToDelete.IsDeleted = true;

                await this.context.SaveChangesAsync();

                return userCardToDelete;

            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException($"Card {cardId} does not exist!", ex);
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetSelectListCards(User user, bool? disabled = null)
        {
            var currentDate = DateTime.Now;

            var userCards = await this.context.CreditCards.Where(c => c.User == user && c.IsDeleted == false)
               .Select(c => new SelectListItem
               {
                   Value = c.Id.ToString(),
                   Text = new string('*', c.CardNumber.Length - 4) + c.LastDigits
                   + " " + (c.ExpiryDate < currentDate
                   ? "expired" : "       "),
                   Disabled = disabled ?? c.ExpiryDate < currentDate
               }).ToListAsync();

            return userCards.ToList();
        }

        public bool DoesCardExist(string cardNumber)
        {
            // if card is found we return false to display that it exists!
            // if card is NOT found with the current card number, we return true so that the method passes
            bool result = this.context.CreditCards.Any(c => c.CardNumber == cardNumber);

            if (result == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsCVVOnlyDigits(string cvv)
        {
            short number;

            bool result = short.TryParse(cvv, out number);

            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsExpired(DateTime expiryDate)
        {
            bool result = expiryDate != null && (expiryDate as DateTime?) > DateTime.Now.AddMonths(1);

            if (result == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
