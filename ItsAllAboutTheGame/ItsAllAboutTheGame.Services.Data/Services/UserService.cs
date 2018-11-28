using ItsAllAboutTheGame.Data;
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
using System.Collections.Generic;
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
        private readonly IWalletService walletService;

        public UserService(ItsAllAboutTheGameDbContext context, UserManager<User> userManager,
            SignInManager<User> signInManager, IForeignExchangeService foreignExchangeService, IMemoryCache cache,
            IWalletService walletService)
        {
            this.cache = cache;
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.foreignExchangeService = foreignExchangeService;
            this.walletService = walletService;
        }

        public async Task<User> RegisterUser(string email, string firstName, string lastName, DateTime dateOfBirth, Currency userCurrency)
        {
            var currentDate = DateTime.Now;

            try
            {
                if (currentDate.Subtract(dateOfBirth).TotalDays < 6575)
                {
                    throw new UserNo18Exception("User must have 18 years to register!");
                }
            }
            catch (UserNo18Exception ex)
            {
                throw new UserNo18Exception(ex.Message);
            }
            

            User user = new User
            {               
                Cards = new List<CreditCard>(),
                Transactions = new List<Transaction>(),
                UserName = email,
                CreatedOn = DateTime.Now,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth              
            };

            Wallet wallet = await walletService.CreateUserWallet(user, userCurrency);
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

            try
            {
                if (currentDate.Subtract(dateOfBirth).TotalDays < 6575)
                {
                    throw new UserNo18Exception("User must have 18 years to register!");
                }
            }
            catch (UserNo18Exception ex)
            {
                throw new UserNo18Exception(ex.Message);
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

            Wallet wallet = await walletService.CreateUserWallet(user, userCurrency);
            user.Wallet = wallet;
            user.WalletId = wallet.Id;

            return user;
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
