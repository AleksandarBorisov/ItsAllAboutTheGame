﻿using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data.DTO;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.Data.Contracts
{
    public interface IWalletService
    {
        Task<Wallet> CreateUserWallet(Currency userCurrency);

        Task<WalletDTO> GetUserWallet(User user);

        Task<WalletDTO> UpdateUserWallet(User user, decimal stake);

        Task<TransactionDTO> WithdrawFromUserBalance(User loggedUser, decimal amount, int cardId);

        Task<decimal> ConvertBalance(User user);
    }
}
