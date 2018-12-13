using ItsAllAboutTheGame.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.Data.Contracts
{
    public interface ICardService
    {
        Task<CreditCard> AddCard(string cardNumber, string cvv, DateTime expiryDate, User user);

        bool DoesCardExist(string cardNumber);

        bool IsCVVOnlyDigits(string cvv);

        bool IsExpired(DateTime expiryDate);

        Task<CreditCard> DeleteCard(int cardId);

        //Task<CreditCard> GetCard(string userId, int cardId);

        //Task<string> GetCardNumber(User user, int cardId);

        //Task<IEnumerable<SelectListItem>> GetSelectListCards(User user, bool? disabled = null);

        //Task<IEnumerable<CreditCard>> GetCards(User user);

        Task<IEnumerable<SelectListItem>> GetSelectListCards(User user, bool? disabled = null);
    }
}
