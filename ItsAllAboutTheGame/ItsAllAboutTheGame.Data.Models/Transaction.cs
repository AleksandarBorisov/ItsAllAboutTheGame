using ItsAllAboutTheGame.Data.Models.Abstract;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItsAllAboutTheGame.Data.Models
{
    public class Transaction : IAuditable, IDeletable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        [Required]
        public Currency Currency { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public string UserId { get; set; }

        [RegularExpression(@"^\d{1,5}(\.\d{1,2})?$", ErrorMessage = "The Amount of the transaction must be valid decimal number with 2 digits after the decimal point")]
        [Column(TypeName = "decimal(18,2)")]
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
