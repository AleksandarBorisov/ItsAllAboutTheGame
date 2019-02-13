using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ItsAllAboutTheGame.Utilities.CustomAttributes.UserAttributes
{
    public class ValidBirthDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object DateOfBirth, ValidationContext validationContext)
        {
            var dateTimeProvider = (IDateTimeProvider)validationContext
                         .GetService(typeof(IDateTimeProvider));

            var isValidDate = DateTime.TryParseExact((string)DateOfBirth,
                       "dd.MM.yyyy",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out DateTime birthDate);

            var now = dateTimeProvider.Now.Year;

            if (!isValidDate)
            {
                return new ValidationResult(ErrorMessage);
            }
            else
            {
                int difference = now - birthDate.Year;

                if (difference < 18 || difference > 100)
                {//You cannot be under 18 years old or over 100 years
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }
        }
    }
}
