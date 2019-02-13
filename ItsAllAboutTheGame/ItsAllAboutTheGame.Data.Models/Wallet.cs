using ItsAllAboutTheGame.GlobalUtilities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "decimal(18,2)")]//18 is total number of digits allowed and 2 is number digits to the right of decimal point
        public decimal Balance { get; set; }
    }
}
