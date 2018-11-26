using ItsAllAboutTheGame.Services.Data.DTO;

namespace ItsAllAboutTheGame.Models.AccountViewModels
{
    public class UserInfoViewModel
    {
        public UserInfoViewModel(UserInfoDTO userInfo)
        {
            this.Balance = userInfo.Balance;
            this.Username = userInfo.Username;
            this.CurrencySymbol = userInfo.CurrencySymbol;
        }

        public decimal Balance { get; set; }

        public string Username { get; set; }

        public string CurrencySymbol { get; set; }
    }
}
