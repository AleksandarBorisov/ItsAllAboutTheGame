using ItsAllAboutTheGame.Data.Models;
using System;
using System.Collections.Generic;

namespace ItsAllAboutTheGame.Services.Data.DTO
{
    public class UserDTO
    {
        public UserDTO(User user)
        {
            this.LockoutFor = user.LockoutEnd;
            this.UserName = user.UserName;
            this.Email = user.Email;
            this.PhoneNumber = user.PhoneNumber;
            this.IsDeleted = user.IsDeleted;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.DateOfBirth = user.DateOfBirth;
            this.Currency = user.Wallet.Currency.ToString();
            this.Balance = user.Wallet.Balance;
            this.Cards = user.Cards;
        }

        public DateTimeOffset? LockoutFor { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsDeleted { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public IEnumerable<CreditCard> Cards { get; set; }
    }
}
