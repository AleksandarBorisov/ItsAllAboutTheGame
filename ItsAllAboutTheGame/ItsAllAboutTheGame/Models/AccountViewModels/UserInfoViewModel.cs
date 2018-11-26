using ItsAllAboutTheGame.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Models.AccountViewModels
{
    public class UserInfoViewModel
    {
        private User user;

        public UserInfoViewModel(User user)
        {
            this.user = user;
            this.Balance = user.Deposit.Balance;
            this.Username = user.UserName;
        }

        public decimal Balance { get; set; }

        public string Username { get; set; }
    }
}
