using ItsAllAboutTheGame.Data.Models.Enums;
using ItsAllAboutTheGame.Data.Models.Utilities.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Models.TransactionViewModels
{
    public class AddCardViewModel
    {
        [Required]
        [MinLength(16), MaxLength(16)]
        public string CardNumber { get; set; }

        [Required]
        [MinLength(3), MaxLength(4)]
        public string CVV { get; set; }

        [Required]
        [FutureDate(ErrorMessage = "Date should be at least 1 month in the future!")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime ExpiryDate { get; set; }
    }
}
