using ItsAllAboutTheGame.Data.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ItsAllAboutTheGame.Data.Models
{
    public class CreditCard : IAuditable, IDeletable
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        [Required]
        [MinLength(16), MaxLength(16)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Card must contain only digits.")]
        public string CardNumber { get; set; }

        [Required]
        [MinLength(3), MaxLength(4)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "CVV must contain only digits.")]
        public string CVV { get; set; }

        public string UserId { get; set; }

        public User CardHolder { get; set; }

        [Required]
        public string CurrencyId { get; set; }

        public Currency Currency { get; set; }

        [Required]   
        [RegularExpression(@"^([1-9]|1[012])$")]
        public int MonthOfExpiration { get; set; }

        [Required]
        public int YearOfExpiration { get; set; }
    }
}
