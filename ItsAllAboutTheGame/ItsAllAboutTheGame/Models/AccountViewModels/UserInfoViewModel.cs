using ItsAllAboutTheGame.Services.Data.DTO;

namespace ItsAllAboutTheGame.Models.AccountViewModels
{
    public class UserInfoViewModel
    {
        public UserInfoViewModel(UserInfoDTO userInfo)
        {
            this.UserId = userInfo.UserId;
            this.Balance = userInfo.Balance;
            this.Username = userInfo.Username;
            this.CurrencySymbol = userInfo.CurrencySymbol;
            this.Admin = userInfo.Admin;
        }
        public bool Admin { get; set; }

        public string UserId { get; set; }

        public decimal Balance { get; set; }

        public string Username { get; set; }

        public string CurrencySymbol { get; set; }
    }
}
