using System;
using System.ComponentModel.DataAnnotations;

namespace ItsAllAboutTheGame.Data.Models.Utilities.CustomAttributes.UserAttributes
{
    public class ValidBirthDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = value != null && ((DateTime)value as DateTime?) <= DateTime.Now.AddYears(-18)
                && (DateTime)value > DateTime.Now.AddYears(-201);
            
            return result;
        }
    }
}
