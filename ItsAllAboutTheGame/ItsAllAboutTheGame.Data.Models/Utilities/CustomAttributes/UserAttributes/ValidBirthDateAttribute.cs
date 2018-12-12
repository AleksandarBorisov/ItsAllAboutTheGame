using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ItsAllAboutTheGame.Data.Models.Utilities.CustomAttributes.UserAttributes
{
    public class ValidBirthDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object DateOfBirth)
        {
            var isValidDate = DateTime.TryParseExact((string)DateOfBirth,
                       "dd.MM.yyyy",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out DateTime birthDate); ;

            var now = DateTime.Now.Year;

            if (!isValidDate)
            {
                return false;
            }
            else
            {
                int difference = now - birthDate.Year;

                if (difference < 18 || difference > 100)
                {//You cannot be under 18 years old or over 100 years
                    return false;
                }

                return true;
            }
        }
    }
}
