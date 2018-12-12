using System;
using System.ComponentModel.DataAnnotations;

namespace ItsAllAboutTheGame.Data.Models.Utilities.CustomAttributes.UserAttributes
{
    public class ValidBirthDateAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validates that the user can not be older than 220 years ( not that it is not possible, just that 
        /// anyone can specify his birth year to be around the time of the pyramids being built)
        /// </summary>
        /// <param name="DateTime.Now.AddYears(-201)"></param>
        /// 
        public override bool IsValid(object value)
        {
            bool result = value != null && ((DateTime)value as DateTime?) <= DateTime.Now.AddYears(-18)
                && (DateTime)value > DateTime.Now.AddYears(-201);
            
            return result;
        }
    }
}
