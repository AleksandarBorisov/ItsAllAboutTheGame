using ItsAllAboutTheGame.Data.Models;
using ItsAllAboutTheGame.Data.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Models.DepositViewModels
{
    public class NewDepositViewModel
    {
        public string CardId { get; set; }

        public List<SelectListItem> Cards { get; set; }

        public decimal Amount { get; set; }

        public Currency CardCurrency { get; set; }
    }
}
