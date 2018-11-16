using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ItsAllAboutTheGame.Data.Models
{
    public class CardCurrency
    {
        [Required]
        public CreditCard CardId { get; set; }

        [Required]
        public Currency CurrencyId { get; set; }
    }
}
