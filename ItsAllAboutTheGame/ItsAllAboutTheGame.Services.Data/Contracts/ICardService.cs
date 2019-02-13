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

        Task<CreditCard> DeleteCard(int cardId);

        Task<IEnumerable<SelectListItem>> GetSelectListCards(User user, bool? disabled = null);
    }
}
