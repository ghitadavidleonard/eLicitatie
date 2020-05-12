using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eLicitatie.Web.Models
{
    public class SignUpViewModel
    {
        [Required]
        [MinLength(3, ErrorMessage = "The username is too short."), MaxLength(255, ErrorMessage = "The username is too long.")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "The username is too short."), MaxLength(255, ErrorMessage = "The username is too long.")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [MinLength(3, ErrorMessage = "The e-mail is too short."), MaxLength(255, ErrorMessage = "The e-mail is too long.")]
        public string EmailAddress { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "The password is too short."), MaxLength(255, ErrorMessage = "The password is too long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "The password should have 1 lower, 1 upper and 1 special character at least and a lenght of 6 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password doesn't match.")]
        public string ConfirmPassword { get; set; }
    }
}
