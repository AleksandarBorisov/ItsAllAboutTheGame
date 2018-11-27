using ItsAllAboutTheGame.Data.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.Data.Contracts
{
    public interface ITransactionService
    {
        Task<Transaction> MakeDeposit(CreditCard creditCard, Wallet wallet, ClaimsPrincipal userClaims);
    }
}
