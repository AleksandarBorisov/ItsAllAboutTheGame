using ItsAllAboutTheGame.Data.Models.Utilities.CustomAttributes.UserAttributes;
using ItsAllAboutTheGame.GlobalUtilities.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ItsAllAboutTheGame.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Firstname cannot be more than 50 characters")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Firstname must countain only letters!")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Lastname cannot be more than 50 characters")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Lastname must countain only letters!")]
        public string LastName { get; set; }

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

        [Required]
        [ValidBirthDate(ErrorMessage = "User must be over 18 years old!")]
        [DataType(DataType.DateTime)]       
        [Display(Name = "Date of birth")]
        [Remote(action: "IsBirthDateValid", controller: "Account", ErrorMessage = "Age must be valid and over 18 years!")]
        public string DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Choose currency for your deposits")]
        public Currency UserCurrency { get; set; }

    }
}
