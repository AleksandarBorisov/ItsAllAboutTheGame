using ItsAllAboutTheGame.Data.Models;
using System;
using System.Linq;

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
            this.Deleted = user.IsDeleted;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.DateOfBirth = user.DateOfBirth.ToString("MM.dd.yyyy");
            this.Currency = user.Wallet.Currency.ToString();
            this.Balance = Math.Round(user.Wallet.Balance, 2);
            this.RegisteredCards = user.Cards.Count();
        }

        public DateTimeOffset? LockoutFor { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool Deleted { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DateOfBirth { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public int RegisteredCards { get; set; }
    }
}
