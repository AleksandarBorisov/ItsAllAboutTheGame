using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Models.TransactionViewModels
{
    public class NewDepositViewModel
    {

        public NewDepositViewModel()
        {

        }


        public Currency CardCurrency { get; set; }
      
        [Required]        
        public int CreditCardId { get; set; }    
        
        public List<SelectListItem> CardsForDelete { get; set; }
                
        public List<SelectListItem> Cards { get; set; }

        [Required]
        [Range(10, 10000)]
        public decimal Amount { get; set; }
    }
}
