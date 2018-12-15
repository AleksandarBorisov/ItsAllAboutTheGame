using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Services.Data.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using X.PagedList;

namespace ItsAllAboutTheGame.Services.Data.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ItsAllAboutTheGameDbContext context;
        private readonly IWalletService walletService;
        private readonly IUserService userService;
        private readonly ICardService cardService;
        private readonly IForeignExchangeService foreignExchangeService;
        private readonly IDateTimeProvider dateTimeProvider;

        public TransactionService(ItsAllAboutTheGameDbContext context, IWalletService walletService,
            IUserService userService, IForeignExchangeService foreignExchangeService,
            ICardService cardService, IDateTimeProvider dateTimeProvider)
        {
            this.context = context;
            this.walletService = walletService;
            this.userService = userService;
            this.cardService = cardService;
            this.dateTimeProvider = dateTimeProvider;
            this.foreignExchangeService = foreignExchangeService;
        }

        public async Task<TransactionDTO> MakeDeposit(User user, int cardId, decimal amount)
        {
            try
            {
                var card = await this.context.CreditCards.FindAsync(cardId);

                if (card.IsDeleted == true)
                {
                    throw new Exception("Card is deleted!");
                }

                var cardLastDigits = card.LastDigits;

                var userWallet = await this.walletService.GetUserWallet(user);

                var rates = await this.foreignExchangeService.GetConvertionRates();

                var convertedAmount = Math.Round(amount / rates.Rates[userWallet.Currency.ToString()], 2);

                user.Wallet.Balance += convertedAmount;

                var transaction = new Transaction()
                {
                    Type = TransactionType.Deposit,
                    Description = GlobalConstants.DepositDescription + cardLastDigits.PadLeft(16, '*'),
                    User = user,
                    UserId = user.Id,
                    Amount = convertedAmount,
                    CreatedOn = dateTimeProvider.Now,
                    Currency = userWallet.Currency
                };

                this.context.Transactions.Add(transaction);

                await this.context.SaveChangesAsync();

                TransactionDTO transactionDTO = new TransactionDTO(transaction);

                return transactionDTO;

            }
            catch (Exception ex)
            {
                throw new EntryPointNotFoundException("Could not make the deposit!", ex);
            }

        }

        public async Task<IPagedList<TransactionDTO>> GetAllTransactions(string searchByUsername = null, int page = 1, int size = GlobalConstants.DefultPageSize, string sortOrder = GlobalConstants.DefaultTransactionSorting)
        {
            var transactions = this.context
                .Transactions
                //.Include(transaction => transaction.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchByUsername))
            {
                transactions = transactions
                    .Where(transaction => transaction.User.UserName.Contains(searchByUsername, StringComparison.InvariantCultureIgnoreCase));
            }

            var property = sortOrder.Remove(sortOrder.IndexOf("_"));
            PropertyInfo prop = typeof(Transaction).GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);

            if (!sortOrder.Contains("_desc"))
            {
                transactions = transactions
                    .OrderBy(transaction => prop.GetValue(transaction));
            }
            else
            {
                transactions = transactions
                   .OrderByDescending(transaction => prop.GetValue(transaction));
            }

            var result = await transactions
                .Select(transaction => new TransactionDTO()
                {
                    Type = transaction.Type,
                    Username = transaction.User.UserName,
                    Amount = Math.Round(transaction.Amount, 2),
                    Description = transaction.Description,
                    CreatedOn = transaction.CreatedOn,
                    Currency = transaction.Currency
                })
                .ToPagedListAsync(page, size);

            return result;
        }

        public async Task<TransactionListDTO> GetUserTransactions(string searchByUsername = null, int page = 1, int size = GlobalConstants.DefultPageSize, string sortOrder = GlobalConstants.DefaultTransactionSorting)
        {
            var transactions = this.context
                .Transactions
                .Include(transaction => transaction.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchByUsername))
            {
                transactions = transactions
                    .Where(transaction => transaction.User.UserName.Contains(searchByUsername ?? "", StringComparison.InvariantCultureIgnoreCase));
            }

            var property = sortOrder.Remove(sortOrder.IndexOf("_"));
            PropertyInfo prop = typeof(Transaction).GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);

            if (!sortOrder.Contains("_desc"))
            {
                transactions = transactions
                    .OrderBy(transaction => prop.GetValue(transaction));
            }
            else
            {
                transactions = transactions
                   .OrderByDescending(transaction => prop.GetValue(transaction));
            }

            var currencies = await this.foreignExchangeService.GetConvertionRates();

            var transactionsDTO = await transactions
                .Select(transaction => new TransactionDTO()
                {
                    Type = transaction.Type,
                    Username = transaction.User.UserName,
                    Amount = Math.Round(transaction.Amount * currencies.Rates[transaction.Currency.ToString()], 2),
                    Description = transaction.Description,
                    CreatedOn = transaction.CreatedOn,
                    Currency = transaction.Currency
                })
                .ToPagedListAsync(page, size);

            var transactionsCurrency = transactions.First().Currency.ToString();

            var getCurrencySymbol = CultureReferences.CurrencySymbols.TryGetValue(transactionsCurrency, out string currencySymbol);

            if (!getCurrencySymbol)
            {
                throw new EntityNotFoundException("Currency with such ISOCurrencySymbol cannot be found");
            }

            var result = new TransactionListDTO(transactionsDTO);

            result.Currency = transactionsCurrency;

            result.CurrencySymbol = currencySymbol;

            return result;
        }

        public async Task<Dictionary<string, decimal>> GetAllAmounts(string searchByUsername = null, string currency = GlobalConstants.BaseCurrency)
        {
            var amountsDictionary = Enum.GetNames(typeof(TransactionType)).ToDictionary(name => name, value => 0m);

            var currencies = await this.foreignExchangeService.GetConvertionRates();

            var currentRate = currencies.Rates[currency];

            var amountsList = await this.context
               .Transactions
               .Include(transaction => transaction.User)
               .Where(transaction => transaction.User.UserName.Contains(searchByUsername ?? "", StringComparison.InvariantCultureIgnoreCase))
               .Select(transaction => new { Type = transaction.Type, Amount = transaction.Amount * currentRate })
               .ToListAsync();

            amountsList.ForEach(amount => amountsDictionary[amount.Type.ToString()] += amount.Amount);

            foreach (var key in Enum.GetNames(typeof(TransactionType)))
            {
                amountsDictionary[key] = Math.Round(amountsDictionary[key], 2);
            }

            return amountsDictionary;
        }

        public async Task<TransactionDTO> GameTransaction(User user, int amount, string game, string description, TransactionType type)
        {
            var wallet = await this.context.Wallets.FirstOrDefaultAsync(k => k.User == user);

            var rates = await this.foreignExchangeService.GetConvertionRates();

            var convertedAmount = Math.Round(amount / rates.Rates[wallet.Currency.ToString()], 2);


            var transaction = new Transaction()
            {
                Type = type,
                Description = description + game,
                User = user,
                UserId = user.Id,
                Amount = convertedAmount,
                CreatedOn = dateTimeProvider.Now,
                Currency = wallet.Currency
            };

            this.context.Transactions.Add(transaction);

            await this.context.SaveChangesAsync();

            var transactionDTO = new TransactionDTO(transaction);

            return transactionDTO;
        }
    }
}
