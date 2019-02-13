using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ItsAllAboutTheGame.Models.TransactionViewModels
{
    public class NewDepositViewModel
    {

        public NewDepositViewModel()
        {

        }

        public string CardCurrencySymbol { get; set; }
      
        [Required]
        [Display(Name = "Choose card")]
        public int? CreditCardId { get; set; }    
        
        public List<SelectListItem> CardsForDelete { get; set; }
                
        public List<SelectListItem> Cards { get; set; }

        [Required]
        [Range(10, 10000)]
        public int? Amount { get; set; }
    }
}
