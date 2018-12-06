using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Enums;

namespace ItsAllAboutTheGame.Services.Data.DTO
{
    public class UserInfoDTO
    {
        public UserInfoDTO()
        {

        }

        public UserInfoDTO(User user, ForeignExchangeDTO currencies)
        {
            this.Balance = user.Wallet.Balance * currencies.Rates[user.Wallet.Currency.ToString()];
            this.Username = user.UserName;
            this.Currency = user.Wallet.Currency.ToString();
            this.UserId = user.Id;
            this.Admin = user.Role.Equals(UserRole.Administrator) || user.Role.Equals(UserRole.MasterAdministrator);
        }

        public bool Admin { get; set; }

        public string UserId { get; set; }

        public decimal Balance { get; set; }

        public string Username { get; set; }

        public string CurrencySymbol { get; set; }

        public string Currency { get; set; }
    }
}
