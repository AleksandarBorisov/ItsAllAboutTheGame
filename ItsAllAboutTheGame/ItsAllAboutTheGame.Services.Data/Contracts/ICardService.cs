using ItsAllAboutTheGame.Data.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.Data.Contracts
{
    public interface ICardService
    {
        Task<CreditCard> AddCard(string cardNumber, string lastDigits, string cvv, DateTime expiryDate, ClaimsPrincipal userClaims);
    }
}
