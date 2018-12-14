using System;
using System.Linq;
using System.Threading.Tasks;
using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
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
        private readonly IDateTimeProvider dateTimeProvider;

        public WalletService(ItsAllAboutTheGameDbContext context, 
            IForeignExchangeService foreignExchangeService, IDateTimeProvider dateTimeProvider)
        {
            this.context = context;
            this.dateTimeProvider = dateTimeProvider;
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

                var getCurrencySymbol = CultureReferences.CurrencySymbols.TryGetValue(userWallet.Currency.ToString(), out string currencySymbol);

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

                var getCurrencySymbol = CultureReferences.CurrencySymbols.TryGetValue(newWallet.Currency.ToString(), out string currencySymbol);

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

        public async Task<TransactionDTO> WithdrawFromUserBalance(User loggedUser, decimal amount, int cardId)
        {
            try
            {
                var card = await this.context.CreditCards.FindAsync(cardId);

                if (card.IsDeleted == true)
                {
                    throw new Exception("Card is deleted!");
                }

                var cardLastDigits = card.LastDigits;

                var user = await this.context.Users.Where(u => u == loggedUser).Include(w => w.Wallet).FirstOrDefaultAsync();

                var userWallet = user.Wallet;               

                var rates =  await this.foreignExchangeService.GetConvertionRates();

                var convertedAmount = amount / rates.Rates[userWallet.Currency.ToString()];

                if (userWallet.Balance - convertedAmount < 0)
                {
                    return new TransactionDTO();
                }
                else
                {
                    userWallet.Balance -= convertedAmount;
                }

                var transaction = new Transaction()
                {
                    Type = TransactionType.Withdraw,
                    Description = GlobalConstants.WithdrawDescription + cardLastDigits.PadLeft(16, '*'),
                    User = user,
                    UserId = user.Id,
                    Amount = convertedAmount,
                    CreatedOn = dateTimeProvider.Now,
                    Currency = userWallet.Currency
                };

                this.context.Transactions.Add(transaction);

                await this.context.SaveChangesAsync();

                var transactionDTO = new TransactionDTO(transaction);

                return transactionDTO;
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException("User wallet not found", ex);
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
