using ItsAllAboutTheGame.Data.Models.Abstract;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<CreditCard> Cards { get; set; }

        public string Image { get; set; }

        [Required]
        public int DayOfBirth { get; set; }

        [Required]
        public int MonthOfBirth { get; set; }

        [Required]
        public int YearOfBirth { get; set; }

        public string UserCurrencyId  { get; set; }

        public UserCurrency UserCurrency { get; set; }
    }
}
