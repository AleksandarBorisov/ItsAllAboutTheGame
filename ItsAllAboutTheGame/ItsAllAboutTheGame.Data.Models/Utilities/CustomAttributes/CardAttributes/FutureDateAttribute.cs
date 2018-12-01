using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ItsAllAboutTheGame.Data.Models.Utilities.CustomAttributes
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = value != null && (DateTime)value > DateTime.Now.AddMonths(1);

            return result;
        }
    }
}
