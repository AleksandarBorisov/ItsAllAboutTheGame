using ItsAllAboutTheGame.Data.Models.Enums;
using ItsAllAboutTheGame.Data.Models.Utilities.CustomAttributes;
using ItsAllAboutTheGame.Data.Models.Utilities.CustomAttributes.UserAttributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ItsAllAboutTheGame.Models.AccountViewModels
{
    public class RegisterViewModel
    {

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required]
        [MaxLength(50, ErrorMessage = "FirstName cannot be more than 50 characters")]
        public string FirstName { get; set; }


        [Required]
        [MaxLength(50, ErrorMessage = "LastName cannot be more than 50 characters")]
        public string LastName { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,20}$", ErrorMessage = "Password must contain at least one non-alphanumerical character, one upper and one lowercase letter and be at least 8 characters long!")]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,20}$", ErrorMessage = "Password must contain at least one non-alphanumerical character, one upper and one lowercase letter and be at least 8 characters long!")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        [Required]
        [ValidBirthDate(ErrorMessage = "User must be over 18 years old!")]
        [DataType(DataType.DateTime)]       
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }


        [Required]
        [Display(Name = "Choose a base currency for your deposits")]
        public Currency UserCurrency { get; set; }

    }
}
