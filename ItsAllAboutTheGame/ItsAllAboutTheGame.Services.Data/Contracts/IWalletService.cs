﻿using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Enums;
using ItsAllAboutTheGame.Services.Data.DTO;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.Data.Contracts
{
    public interface IWalletService
    {
        Task<Wallet> CreateUserWallet(User user, Currency userCurrency);

        Task<WalletDTO> GetUserWallet(User user);

        Task<WalletDTO> UpdateUserWallet(User user, int stake);

        Task<decimal> ConvertBalance(User user);
    }
}
