using ItsAllAboutTheGame.Data;
using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Enums;
using ItsAllAboutTheGame.Services.Data.Contracts;
using System;

namespace ItsAllAboutTheGame.Services.Data
{
    public class UserService : IUserService
    {
        private readonly ItsAllAboutTheGameDbContext _context;

        public UserService(ItsAllAboutTheGameDbContext context)
        {
            this._context = context;
        }


        public User RegisterUser(string email, string firstName, string lastName, DateTime dateOfBirth)
        {
            if (firstName == null || lastName == null || dateOfBirth == null)
            {
                throw new ArgumentNullException("No parameter can be null");
            }

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

            Deposit deposit = new Deposit
            {
                User = user,
                Currency = Currency.EUR,
                Balance = 0
            };

            user.Deposit = deposit;

            return user;
        }
    }
}
