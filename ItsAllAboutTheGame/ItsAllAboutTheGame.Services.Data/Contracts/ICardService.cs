using ItsAllAboutTheGame.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.Data.Contracts
{
    public interface ICardService
    {
        Task<CreditCard> AddCard(string cardNumber, string cvv, DateTime expiryDate, User user);

        bool DoesCardExist(string cardNumber);

        bool AreOnlyDigits(string cvv);

        bool IsExpired(DateTime expiryDate);

        Task<CreditCard> GetCard(User user, int cardId);

        Task<string> GetCardNumber(string cardNumber);

        Task<string> GetCardNumber(User user, int cardId);

        Task<IEnumerable<SelectListItem>> GetSelectListCards(User user);

        Task<IEnumerable<CreditCard>> GetCards(User user);
    }
}
