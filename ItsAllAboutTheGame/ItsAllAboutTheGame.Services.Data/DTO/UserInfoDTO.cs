using ItsAllAboutTheGame.Data.Models;

namespace ItsAllAboutTheGame.Services.Data.DTO
{
    public class UserInfoDTO
    {
        public UserInfoDTO()
        {
            
        }

        public decimal Balance { get; set; }

        public string Username { get; set; }

        public string CurrencySymbol { get; set; }

        public string Currency { get; set; }
    }
}
