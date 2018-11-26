using ItsAllAboutTheGame.Data.Models;

namespace ItsAllAboutTheGame.Models.AccountViewModels
{
    public class UserInfoViewModel
    {
        public UserInfoViewModel(User user, string currencySymbol)
        {
            this.Balance = user.Wallet.Balance;
            this.Username = user.UserName;
            this.CurrencySymbol = currencySymbol;
        }

        public decimal Balance { get; set; }

        public string Username { get; set; }

        public string CurrencySymbol { get; set; }
    }
}
