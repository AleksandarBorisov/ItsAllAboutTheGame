using System;
using System.ComponentModel.DataAnnotations;

namespace ItsAllAboutTheGame.Utilities.CustomAttributes.GameAttributes
{
    public class ValidStakeAttribute : ValidationAttribute
    {
        private readonly string comparisonProperty;

        public ValidStakeAttribute(string comparisonProperty)
        {
            this.comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (int)value;

            var property = validationContext.ObjectType.GetProperty(this.comparisonProperty);

            if (property == null)
                throw new ArgumentException("Property with this name not found");

            int comparisonValue = (int)Math.Floor((decimal)property.GetValue(validationContext.ObjectInstance));

            if (currentValue == 0 || currentValue > comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
