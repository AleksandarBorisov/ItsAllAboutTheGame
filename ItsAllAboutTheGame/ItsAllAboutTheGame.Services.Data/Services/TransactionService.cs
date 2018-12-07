using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Enums;
using ItsAllAboutTheGame.GlobalUtilities.Constants;
using ItsAllAboutTheGame.Services.Data.Constants;
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

            var convertedamount = amount / rates.Rates[userWallet.Currency.ToString()];

             user.Wallet.Balance += convertedamount;

            var transaction =   new Transaction()
            {
                Type = TransactionType.Deposit,
                Description = ServicesDataConstants.DepositDescription + userCardNumber,
                User = user,
                UserId = user.Id,
                Amount = convertedamount,
                CreatedOn = DateTime.Now
                //TODO: Mock DateTime
            };

            this.context.Transactions.Add(transaction);
            await this.context.SaveChangesAsync();

            return transaction;
        }

        public IPagedList<TransactionDTO> GetAllTransactions(string searchByUsername = null, int page = 1, int size = GlobalConstants.DefultPageSize, string sortOrder = GlobalConstants.DefultTransactionSorting)
        {
            var transactions = this.context
                .Transactions
                .Include(transaction => transaction.User)
                .Select(u => new TransactionDTO(u));

            var property = sortOrder.Remove(sortOrder.IndexOf("_"));
            PropertyInfo prop = typeof(UserDTO).GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
            if (!sortOrder.Contains("_desc"))
            {
                transactions = transactions.OrderBy(transaction => prop.GetValue(transaction));
            }
            else
            {
                transactions = transactions.OrderByDescending(user => prop.GetValue(user));
            }

            if (!string.IsNullOrEmpty(searchByUsername))
            {
                transactions = transactions.Where(user => user.Username.Contains(searchByUsername, StringComparison.InvariantCultureIgnoreCase));
            }

            return transactions.ToPagedList(page, size);
        }
    }
}
