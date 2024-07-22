using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialClub.Models
{
    public class PassValidate
    {
        [Required(ErrorMessage = "Please Enter Password.")]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "Please Enter Confirm Password.")]
        [Compare("PasswordHash", ErrorMessage = "Password & Confirm Password does not match")]
        public string ConfirmPasswordHash { get; set; }
        [Required(ErrorMessage = "Please Enter Old Password.")]
        public string CurrentPasswordHash { get; set; }
        [Required(ErrorMessage = "Please Enter Email.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}