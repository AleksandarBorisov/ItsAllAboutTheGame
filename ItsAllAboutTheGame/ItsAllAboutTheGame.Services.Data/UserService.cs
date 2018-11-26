﻿using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Enums;
using ItsAllAboutTheGame.Services.Data.Constants;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Services.Data.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.Data
{
    public class UserService : IUserService
    {
        private readonly IForeignExchangeService foreignExchangeService;
        private readonly ItsAllAboutTheGameDbContext context;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IMemoryCache cache;

        public UserService(ItsAllAboutTheGameDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, IForeignExchangeService foreignExchangeService, IMemoryCache cache)
        {
            this.cache = cache;
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.foreignExchangeService = foreignExchangeService;
        }

        public async Task<User> RegisterUser(string email, string firstName, string lastName, DateTime dateOfBirth, Currency userCurrency)
        {
            var currentDate = DateTime.Now;

            if (currentDate.Subtract(dateOfBirth).TotalDays < 6575)
            {
                // proper dispaly page must be shown to user if he doesnt have 18 years old
            }

            User user = new User
            {
                UserName = email,
                CreatedOn = DateTime.Now,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth
            };

            Wallet wallet = await CreateUserWallet(user, userCurrency);
            user.Wallet = wallet;
            user.WalletId = wallet.Id;

            return user;
        }

        public async Task<User> RegisterUserWithLoginProvider(ExternalLoginInfo info, Currency userCurrency, DateTime dateOfBirth)
        {
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name).Split().ToArray();
            string firstName = name[0];
            string lastName = name[1];

            var currentDate = DateTime.Now;

            if (currentDate.Subtract(dateOfBirth).TotalDays < 6575)
            {
                throw new ArgumentException("User must be over 18 years old");
            }

            User user = new User
            {
                UserName = email,
                CreatedOn = DateTime.Now,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth
            };

            Wallet wallet = await CreateUserWallet(user, userCurrency);
            user.Wallet = wallet;
            user.WalletId = wallet.Id;

            return user;
        }


        private async Task<Wallet> CreateUserWallet(User user, Currency userCurrency)
        {
            Wallet wallet = new Wallet
            {
                Currency = userCurrency,
                Balance = 0
            };

            this.context.Wallets.Add(wallet);
            await this.context.SaveChangesAsync();

            return wallet;
        }

        public async Task<UserInfoDTO> GetUserInfo(ClaimsPrincipal userClaims)
        {
            var userId = userManager.GetUserId(userClaims);

            var currencies = await this.cache.GetOrCreateAsync("ConvertionRates", entry =>
            {
                entry.AbsoluteExpiration = DateTime.UtcNow.AddDays(1);
                return this.foreignExchangeService.GetConvertionRates();
            });

            var user = await userManager.Users.Where(x => x.Id.Equals(userId))
                .Include(player => player.Wallet)
                .Select(u => new UserInfoDTO
                {
                    Balance = u.Wallet.Balance * currencies.Rates[u.Wallet.Currency.ToString()],
                    Username = u.UserName,
                    IsSignedIn = this.signInManager.IsSignedIn(userClaims),
                    Currency = u.Wallet.Currency.ToString()
                })
                .FirstOrDefaultAsync();

            Math.Round(user.Balance, 2);

            var getCurrencySymbol = ServicesDataConstants.CurrencySymbols.TryGetValue(user.Currency, out string currencySymbol);
            if (!getCurrencySymbol)
            {
                throw new EntityNotFoundException("Currency with such ISOCurrencySymbol cannot be found");
            }
            user.CurrencySymbol = currencySymbol;

            return user;
        }
    }
}
