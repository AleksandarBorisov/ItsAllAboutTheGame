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
        public string CardId { get; set; }
        
        [Required]
        public List<SelectListItem> Cards { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public Currency CardCurrency { get; set; }
    }
}
