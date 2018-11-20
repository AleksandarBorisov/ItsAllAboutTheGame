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

        [Range(1,31, ErrorMessage = "Day must be an integer number between 1 and 31")]
        public int DayOfBirth { get; set; }

        [Range(1,12, ErrorMessage = "Month must be an integer number between 1 and 12")]
        public int MonthOfBirth { get; set; }

        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Year must be a four digit integer number")]
        public int YearOfBirth { get; set; }

        [Required]
        public Deposit Deposit { get; set; }

        public int DepositId { get; set; }
    }
}
