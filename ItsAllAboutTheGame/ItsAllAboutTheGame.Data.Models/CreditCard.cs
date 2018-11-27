using ItsAllAboutTheGame.Data.Models.Abstract;
using ItsAllAboutTheGame.Data.Models.Utilities.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace ItsAllAboutTheGame.Data.Models
{
    public class CreditCard : IAuditable, IDeletable
    {//Every Card's currecny will be in USD and cannot be changed, because it comes from the PaySave API

        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        [MinLength(16),MaxLength(16)]
        public string CardNumber { get; set; }

        [Required]
        [MinLength(4), MaxLength(4)]
        public string LastDigits { get; set; }

        [Required]
        [FutureDate(ErrorMessage = "Date should be at least 1 month in the future!")]
        public DateTime ExpiryDate { get; set; }
     
        [Required]
        [MinLength(3),MaxLength(4)]
        public string CVV { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }
    }
}
