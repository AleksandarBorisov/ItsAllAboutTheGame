using ExpressiveAnnotations.Attributes;
using ItsAllAboutTheGame.Data.Models.Enums;
using ItsAllAboutTheGame.Services.Data.DTO;
using System.ComponentModel.DataAnnotations;

namespace ItsAllAboutTheGame.Models.GameViewModels
{
    public class WalletViewModel
    {
        public WalletViewModel()
        {

        }

        public WalletViewModel(WalletDTO wallet)
        {
            this.Balance = (int)wallet.Balance;
            this.Currency = wallet.Currency;
            this.CurrencySymbol = wallet.CurrencySymbol;
        }
        
        public int Balance { get; set; }

        public Currency Currency { get; set; }

        public string CurrencySymbol { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid positive Page Number.")]
        [AssertThat("Stake <= Balance", ErrorMessage = "You cannot stake more than you have in your deposit!")]
        public int? Stake { get; set; }

        public int Win { get; set; }
    }
}
