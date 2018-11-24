using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Enums;
using ItsAllAboutTheGame.Services.Data.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Services.Data
{
    public class UserService : IUserService
    {
        private readonly ItsAllAboutTheGameDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(ItsAllAboutTheGameDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
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


            Deposit deposit = await CreateUserDeposit(user, userCurrency);
            user.Deposit = deposit;
            user.DepositId = deposit.Id;

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

            Deposit deposit = await CreateUserDeposit(user, userCurrency);
            user.Deposit = deposit;
            user.DepositId = deposit.Id;

            return user;
        }
       

        private async Task<Deposit> CreateUserDeposit(User user, Currency userCurrency)
        {
            Deposit deposit = new Deposit
            {                
                Currency = userCurrency,
                Balance = 0
            };


            this._context.Deposits.Add(deposit);
            await this._context.SaveChangesAsync();


            return deposit;
        }
    }
}
