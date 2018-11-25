using ItsAllAboutTheGame.Data.Models.Abstract;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ItsAllAboutTheGame.Data.Models
{
    public class User : IdentityUser, IAuditable, IDeletable
    {
        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "FirstName cannot be more than 50 characters")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "LastName cannot be more than 50 characters")]
        public string LastName { get; set; }

        public IEnumerable<CreditCard> Cards { get; set; }

        public IEnumerable<Transaction> Transactions { get; set; }

        public string Image { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Wallet Wallet { get; set; }

        public int WalletId { get; set; }
    }
}
