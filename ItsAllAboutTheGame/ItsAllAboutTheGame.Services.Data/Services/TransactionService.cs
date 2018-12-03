using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Enums;
using ItsAllAboutTheGame.Services.Data.Constants;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
    }
}
