using ItsAllAboutTheGame.GlobalUtilities.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ItsAllAboutTheGame.Utilities.CustomAttributes.CardAttributes
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateTimeProvider = (IDateTimeProvider)validationContext
                         .GetService(typeof(IDateTimeProvider));

            var dateStrings = value.ToString().Split('/');

            if (dateStrings.Length < 2 || !int.TryParse(dateStrings[0], out int month) || !int.TryParse(dateStrings[0], out int year))
            {
                return new ValidationResult(ErrorMessage);
            }

            var isValidDate = DateTime.TryParseExact($"01.{dateStrings[0]}.20{dateStrings[1]}",
                       "dd.MM.yyyy",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out DateTime expiryDateAsDate);

            var now = dateTimeProvider.Now;

            if (!isValidDate)
            {
                return new ValidationResult(ErrorMessage);
            }
            else
            {
                if (now < expiryDateAsDate)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(ErrorMessage);
            }
        }
    }
}
