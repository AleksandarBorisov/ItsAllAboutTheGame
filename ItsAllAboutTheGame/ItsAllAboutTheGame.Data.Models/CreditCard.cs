using ItsAllAboutTheGame.Data.Models.Abstract;
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
        public string PaymentToken { get; set; }

        public string CardName { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }
    }
}
