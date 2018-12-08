using ItsAllAboutTheGame.Data.Models.Enums;
using ItsAllAboutTheGame.Services.Data.DTO;
using ItsAllAboutTheGame.Utilities.CustomAttributes.GameAttributes;
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
            this.Balance = wallet.Balance;
            this.Currency = wallet.Currency;
            this.CurrencySymbol = wallet.CurrencySymbol;
        }
        
        public decimal Balance { get; set; }

        public Currency Currency { get; set; }

        public string CurrencySymbol { get; set; }

        //[AssertThat("Stake < Balance", ErrorMessage = "You cannot stake more than you have in your deposit!")]
        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid positive Page Number.")]
        [ValidStake("Balance",ErrorMessage = "You cannot stake more than you have in your deposit!")]
        public int? Stake { get; set; }

        public decimal Win { get; set; }
    }
}
