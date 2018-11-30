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
        private readonly ServicesDataConstants constants;

        public TransactionService(ItsAllAboutTheGameDbContext context, IWalletService walletService,
            UserManager<User> userManager, IUserService userService, ServicesDataConstants constants)
        {
            this.context = context;
            this.walletService = walletService;
            this.userManager = userManager;
            this.userService = userService;
            this.constants = constants;
        }

        //public async Task<Transaction> MakeDeposit(CreditCard creditCard, ClaimsPrincipal userClaims, decimal amount)
        //{
        //    var userName = userClaims.Identity.Name;
        //    var user = await this.userService.GetUser(userName);

        //    var userCard = await this.context.CreditCards.Where(k => k.User == user && k.LastDigits == creditCard.LastDigits)
        //        .Select(n => n.CardNumber).FirstOrDefaultAsync();

        //    this.walletService.IncrementUserWallet(user, amount);

        //    var rates = this.constants.ConvertionRates();
        //    var convertedAmount = amount / rates.Result.Rates[user.Wallet.Currency.ToString()];


        //    var transaction = new Transaction()
        //    {
        //        Type = TransactionType.Deposit,
        //        Description = constants.DepositDescription + userCard,
        //        User = user,
        //        UserId = user.Id,
        //        Amount =  convertedAmount,
        //        CreatedOn = DateTime.Now
        //    };

        //    return transaction;
        //}
    }
}
