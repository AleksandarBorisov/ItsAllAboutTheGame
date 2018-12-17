using ItsAllAboutTheGame.GlobalUtilities.Enums;
using ItsAllAboutTheGame.Utilities.CustomAttributes.UserAttributes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ItsAllAboutTheGame.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [Display(Name = "Choose a base currency for your deposits")]
        public Currency UserCurrency { get; set; }

        [Required]
        [ValidBirthDate(ErrorMessage = "User must be over 18 years old!")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date of birth")]
        [Remote(action: "IsBirthDateValid", controller: "Account", ErrorMessage = "Age must be valid and over 18 years!")]
        public string DateOfBirth { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,20}$", ErrorMessage = "[A-Z,a-z,0-9]!@#&() and at least 8 symbols!")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,20}$", ErrorMessage = "[A-Z,a-z,0-9]!@#&() and at least 8 symbols!")]
        [Compare("Password", ErrorMessage = "Must match with password.")]
        public string ConfirmPassword { get; set; }
    }
}
