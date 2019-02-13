using ItsAllAboutTheGame.Utilities.CustomAttributes.CardAttributes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ItsAllAboutTheGame.Models.TransactionViewModels
{
    public class AddCardViewModel
    {
        [Required]
        [MinLength(19, ErrorMessage = "Card number must be 16 digits!"), MaxLength(19, ErrorMessage = "Card number must be 16 digits!")]
        [RegularExpression("[0-9 ]+", ErrorMessage = "Card number should contain digits only!")]
        [Remote(action: "DoesExist", controller: "Transaction", ErrorMessage = "Card already exists in the system!")]
        public string CardNumber { get; set; }

        [Required]
        [MinLength(3), MaxLength(4)]
        [RegularExpression("[0-9]+", ErrorMessage = "CVV should contain digits only!")]
        public string CVV { get; set; }

        [Required]
        [Remote(action: "IsDateValid", controller: "Transaction", ErrorMessage = "Please enter valid future expiry date for your card!")]
        [FutureDate(ErrorMessage = "Please enter valid future date!")]
        public string ExpiryDate { get; set; }
    }
}
