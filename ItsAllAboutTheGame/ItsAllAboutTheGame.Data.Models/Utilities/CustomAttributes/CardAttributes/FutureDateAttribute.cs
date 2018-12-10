using System;
using System.ComponentModel.DataAnnotations;

namespace ItsAllAboutTheGame.Data.Models.Utilities.CustomAttributes
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = value != null && ((DateTime)value as DateTime?) > DateTime.Now.AddMonths(1);

            return result;
        }
    }
}
