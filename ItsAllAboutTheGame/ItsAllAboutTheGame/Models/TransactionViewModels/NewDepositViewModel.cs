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
        [Required]
        public Currency CardCurrency { get; set; }

        public int CardId { get; set; }       

        [Required]
        public IEnumerable<SelectListItem> Cards { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
