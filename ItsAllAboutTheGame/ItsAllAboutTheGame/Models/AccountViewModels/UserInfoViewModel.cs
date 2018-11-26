using ItsAllAboutTheGame.Data.Models;

namespace ItsAllAboutTheGame.Models.AccountViewModels
{
    public class UserInfoViewModel
    {
        public UserInfoViewModel(User user)
        {
            this.Balance = user.Wallet.Balance;
            this.Username = user.UserName;
        }

        public decimal Balance { get; set; }

        public string Username { get; set; }
    }
}
