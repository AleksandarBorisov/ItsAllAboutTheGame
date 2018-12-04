using ItsAllAboutTheGame.Data.Models;
using System;
using System.Linq;

namespace ItsAllAboutTheGame.Services.Data.DTO
{
    public class UserDTO
    {
        public UserDTO()
        {

        }

        public UserDTO(User user)
        {
            this.UserId = user.Id;
            this.LockoutFor = GetLockoutDays(user);
            this.Username = user.UserName;
            this.Email = user.Email;
            this.PhoneNumber = user.PhoneNumber;
            this.Deleted = user.IsDeleted;
            this.Firstname = user.FirstName;
            this.Lastname = user.LastName;
            this.DateOfBirth = user.DateOfBirth.ToString("MM.dd.yyyy");
            this.Currency = user.Wallet.Currency.ToString();
            this.Balance = Math.Round(user.Wallet.Balance, 2);
            this.RegisteredCards = user.Cards.Count();
        }

        public string UserId { get; set; }

        public int LockoutFor { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool Deleted { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string DateOfBirth { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public int RegisteredCards { get; set; }

        public bool Admin { get; set; }

        private int GetLockoutDays(User user)
        {
            if (user.LockoutEnd == null)
            {
                return 0;
            }

            var lockOutEndDate = user.LockoutEnd.Value.DateTime;

            var currentDate = DateTime.Now;

            var difference = (lockOutEndDate - currentDate).TotalDays;

            return (int)(difference > 0 ? Math.Round(difference, 1) : 0);
        }
    }
}
