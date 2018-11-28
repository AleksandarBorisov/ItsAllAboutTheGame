using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.Data.Contracts
{
    public interface IWalletService
    {
        Task<Wallet> CreateUserWallet(User user, Currency userCurrency);

        Task<Wallet> GetUserWallet(User user);
    }
}
