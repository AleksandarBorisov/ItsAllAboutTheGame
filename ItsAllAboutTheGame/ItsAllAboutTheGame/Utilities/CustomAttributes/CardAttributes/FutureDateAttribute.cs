using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ItsAllAboutTheGame.Utilities.CustomAttributes.CardAttributes
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var dateStrings = value.ToString().Split('/');

            if (dateStrings.Length < 2 || !int.TryParse(dateStrings[0], out int month) || !int.TryParse(dateStrings[0], out int year))
            {
                return false;
            }

            var isValidDate = DateTime.TryParseExact($"01.{dateStrings[0]}.20{dateStrings[1]}",
                       "dd.MM.yyyy",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out DateTime expiryDateAsDate);

            var now = DateTime.Now;

            if (!isValidDate)
            {
                return false;
            }
            else
            {
                if (now < expiryDateAsDate)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
