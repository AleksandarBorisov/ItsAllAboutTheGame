﻿using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Enums;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.Data
{
    public class WalletService : IWalletService
    {
        private readonly IMemoryCache cache;
        private readonly ItsAllAboutTheGameDbContext context;

        public WalletService(IMemoryCache cache, ItsAllAboutTheGameDbContext context)
        {
            this.cache = cache;
            this.context = context;
        }

        public async Task<Wallet> CreateUserWallet(User user, Currency userCurrency)
        {
            Wallet wallet = new Wallet
            {
                Currency = userCurrency,
                Balance = 0
            };

            context.Wallets.Add(wallet);
            await context.SaveChangesAsync();

            return wallet;
        }

        public async Task<Wallet> GetUserWallet(User user)
        {
            var userWallet = await this.context.Wallets.Where(u => u.UserId == user.Id).FirstOrDefaultAsync();

            return userWallet;
        }
    }
}
