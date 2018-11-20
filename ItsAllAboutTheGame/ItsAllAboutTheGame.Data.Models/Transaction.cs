using ItsAllAboutTheGame.Data.Models.Abstract;
using ItsAllAboutTheGame.Data.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ItsAllAboutTheGame.Data.Models
{
    public class Transaction : IAuditable, IDeletable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public string UserId { get; set; }

        [RegularExpression(@"^\d{1,5}(\.\d{1,2})?$", ErrorMessage = "The Amount of the transaction must be valid decimal number with 2 digits after the decimal point")]
        public decimal Amount { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }
    }
}
