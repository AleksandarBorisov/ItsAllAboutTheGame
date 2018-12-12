using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace ItsAllAboutTheGame.Models.TransactionViewModels
{
    public class AddCardViewModel
    {
        [Required]
        [MinLength(16), MaxLength(16)]
        [RegularExpression("[0-9]+", ErrorMessage = "Card number should contain digits only!")]
        [Remote(action: "DoesExist", controller: "Transaction", ErrorMessage = "Card already exists in the system!")]
        public string CardNumber { get; set; }

        [Required]
        [MinLength(3), MaxLength(4)]
        [RegularExpression("[0-9]+", ErrorMessage = "Card number should contain digits only!")]
        [Remote(action: "AreDigits", controller: "Transaction", ErrorMessage = "CVV must contain only digits!")]
        public string CVV { get; set; }
        
        [Required]
        [Remote(action: "IsDateValid", controller: "Transaction", ErrorMessage = "Card expiry date must be at least 1 month in the future!")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime ExpiryDate { get; set; }
    }
}
