using System.ComponentModel.DataAnnotations;

namespace ItsAllAboutTheGame.Areas.Administration.Models.UserViewModels
{
    public class CardViewModel
    {
        [Required(ErrorMessage = "Card Number is required")]
        [MinLength(16), MaxLength(16)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Card must contain only digits.")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "CVV is required")]
        [MinLength(3), MaxLength(4)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "CVV must contain only digits.")]
        public string CVV { get; set; }

        [Required(ErrorMessage = "Expiration Month is required")]
        [RegularExpression(@"^([1-9]|1[012])$", ErrorMessage = "Month must be valid integer between 1 and 12.")]
        public int MonthOfExpiration { get; set; }

        [Required(ErrorMessage = "Expiration Year is required")]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Year must be a four digit integer number between now and 20 years in the future.")]
        public int YearOfExpiration { get; set; }
    }
}
