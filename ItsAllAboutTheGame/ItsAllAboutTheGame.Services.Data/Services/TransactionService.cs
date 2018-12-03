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
        private readonly IForeignExchangeService foreignExchangeService;

        public TransactionService(ItsAllAboutTheGameDbContext context, IWalletService walletService,
            UserManager<User> userManager, IUserService userService, IForeignExchangeService foreignExchangeService)
        {
            this.context = context;
            this.walletService = walletService;
            this.userManager = userManager;
            this.userService = userService;
            this.foreignExchangeService = foreignExchangeService;
        }

        public async Task<Transaction> MakeDeposit(CreditCard creditcard, ClaimsPrincipal userclaims, decimal amount)
        {
            var username = userclaims.Identity.Name;
            var user = await this.userService.GetUser(username);

    
            var usercard =  user.Cards
                .Where(k => k.LastDigits == creditcard.LastDigits)
                .Select(n => n.CardNumber).FirstOrDefault();

           

            var rates = await this.foreignExchangeService.GetConvertionRates();
            var convertedamount = amount / rates.Rates[user.Wallet.Currency.ToString()];

             //this.walletService.IncrementUserWallet(user, convertedamount);
             user.Wallet.Balance += convertedamount;

            var transaction =  new Transaction()
            {
                Type = TransactionType.Deposit,
                Description = ServicesDataConstants.DepositDescription + usercard,
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
