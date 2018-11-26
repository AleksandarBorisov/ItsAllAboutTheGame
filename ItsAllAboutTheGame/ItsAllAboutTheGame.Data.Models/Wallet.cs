using ItsAllAboutTheGame.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ItsAllAboutTheGame.Data.Models
{
    public class Wallet
    {
        [Key]
        public int Id { get; set; }
        
        public string UserId { get; set; }
       
        public User User { get; set; }

        [Required]
        public Currency Currency { get; set; }

        [RegularExpression(@"^\d{1,5}(\.\d{1,2})?$", ErrorMessage = "Balance of the wallet must be valid decimal number with 2 digits after the decimal point")]
        public decimal Balance { get; set; }
    }
}
