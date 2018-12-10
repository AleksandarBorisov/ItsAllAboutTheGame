using System;
using System.Linq;
using System.Threading.Tasks;
using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Enums;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.Services.Data.Constants;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Services.Data.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ItsAllAboutTheGame.Services.Data
{
    public class WalletService : IWalletService
    {
        private readonly ItsAllAboutTheGameDbContext context;
        private readonly IForeignExchangeService foreignExchangeService;

        public WalletService(ItsAllAboutTheGameDbContext context, IForeignExchangeService foreignExchangeService)
        {
            this.context = context;
            this.foreignExchangeService = foreignExchangeService;
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

        public async Task<WalletDTO> GetUserWallet(User user)
        {
            try
            {
                var userWallet = await this.context.Wallets.FirstOrDefaultAsync(k => k.User == user);

                var getCurrencySymbol = ServicesDataConstants.CurrencySymbols.TryGetValue(userWallet.Currency.ToString(), out string currencySymbol);

                if (!getCurrencySymbol)
                {
                    throw new EntityNotFoundException("Currency with such ISOCurrencySymbol cannot be found");
                }

                var currencies = await this.foreignExchangeService.GetConvertionRates();

                var wallet = new WalletDTO(userWallet, currencies);

                wallet.CurrencySymbol = currencySymbol;

                return wallet;
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException("Cannot find the specified Wallet", ex);
            }
        }

        public async Task<WalletDTO> UpdateUserWallet(User user, decimal stake)
        {
            try
            {
                var oldWallet = await this.context.Wallets.FirstOrDefaultAsync(k => k.User == user);

                var currencies = await this.foreignExchangeService.GetConvertionRates();

                oldWallet.Balance += stake / currencies.Rates[oldWallet.Currency.ToString()];

                this.context.Wallets.Update(oldWallet);

                await this.context.SaveChangesAsync();

                var newWallet = new WalletDTO(oldWallet, currencies);

                var getCurrencySymbol = ServicesDataConstants.CurrencySymbols.TryGetValue(newWallet.Currency.ToString(), out string currencySymbol);

                if (!getCurrencySymbol)
                {
                    throw new EntityNotFoundException("Currency with such ISOCurrencySymbol cannot be found");
                }

                newWallet.CurrencySymbol = currencySymbol;

                return newWallet;
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException("Cannot find the specified Wallet", ex);
            }
        }


        public async Task<TransactionDTO> WithdrawFromUserBalance(string userId, decimal amount)
        {
            try
            {
                var user = await this.context.Users.Where(u => u.Id == userId).Include(w => w.Wallet).FirstOrDefaultAsync();

                var userWallet = user.Wallet;               

                var rates =  await this.foreignExchangeService.GetConvertionRates();

                var convertedAmount = amount / rates.Rates[userWallet.Currency.ToString()];

                userWallet.Balance -= convertedAmount;

                var transaction = new Transaction()
                {
                    Type = TransactionType.Withdraw,
                    Description = GlobalConstants.WithdrawDescription + userWallet.Currency + amount,
                    User = user,
                    UserId = user.Id,
                    Amount = convertedAmount,
                    CreatedOn = DateTime.Now
                    //TODO: Mock DateTime
                };

                this.context.Transactions.Add(transaction);

                await this.context.SaveChangesAsync();

                var transactionDTO = new TransactionDTO(transaction);

                return transactionDTO;
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException("User wallet not found");
            }
        }


        public async Task<decimal> ConvertBalance(User user)
        {
            var userWallet = await GetUserWallet(user);

            var currencies = await this.foreignExchangeService.GetConvertionRates();

            var rate = currencies.Rates[userWallet.Currency.ToString()];

            var resultAmount = user.Wallet.Balance * rate;

            return resultAmount;
        }
    }
}
