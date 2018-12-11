using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Services.Data.Contracts;
using ItsAllAboutTheGame.Services.Data.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using X.PagedList;

namespace ItsAllAboutTheGame.Services.Data.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ItsAllAboutTheGameDbContext context;
        private readonly UserManager<User> userManager;
        private readonly IWalletService walletService;
        private readonly IUserService userService;
        private readonly ICardService cardService;
        private readonly IForeignExchangeService foreignExchangeService;

        public TransactionService(ItsAllAboutTheGameDbContext context, IWalletService walletService,
            UserManager<User> userManager, IUserService userService, IForeignExchangeService foreignExchangeService,
            ICardService cardService)
        {
            this.context = context;
            this.walletService = walletService;
            this.userManager = userManager;
            this.userService = userService;
            this.cardService = cardService;
            this.foreignExchangeService = foreignExchangeService;
        }

        public async Task<Transaction> MakeDeposit(User user, int cardId, decimal amount)
        {
            var userCardNumber = await this.cardService.GetCardNumber(user, cardId);

            var userWallet = await this.walletService.GetUserWallet(user);

            var rates = await this.foreignExchangeService.GetConvertionRates();

            var convertedAmount = Math.Round(amount / rates.Rates[userWallet.Currency.ToString()], 2);

            user.Wallet.Balance += convertedAmount;

            var transaction = new Transaction()
            {
                Type = TransactionType.Deposit,
                Description = GlobalConstants.DepositDescription + userCardNumber,
                User = user,
                UserId = user.Id,
                Amount = convertedAmount,
                CreatedOn = DateTime.Now
                //TODO: Mock DateTime
            };

            this.context.Transactions.Add(transaction);
            await this.context.SaveChangesAsync();

            return transaction;
        }

        public async Task<TransactionListDTO> GetAllTransactions(string searchByUsername = null, int page = 1, int size = GlobalConstants.DefultPageSize, string sortOrder = GlobalConstants.DefultTransactionSorting)
        {
            var transactions = await this.context
                .Transactions
                .Include(transaction => transaction.User)
                .Where(transaction => transaction.User.UserName.Contains(searchByUsername ?? "", StringComparison.InvariantCultureIgnoreCase))
                .ToPagedListAsync(page, size);

            var result = new TransactionListDTO(transactions, sortOrder);

            return result;
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
                CreatedOn = DateTime.Now
            };

            this.context.Transactions.Add(transaction);

            await this.context.SaveChangesAsync();

            var transactionDTO = new TransactionDTO(transaction);

            return transactionDTO;
        }
    }
}
