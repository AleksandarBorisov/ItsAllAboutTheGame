using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ItsAllAboutTheGame.Data.Models
{
    public class UserCurrency
    {
        [Required]
        public User UserId { get; set; }

        [Required]
        public Currency CurrencyId { get; set; }
    }
}
