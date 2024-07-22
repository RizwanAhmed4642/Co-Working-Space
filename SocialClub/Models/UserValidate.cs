using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialClub.Models
{
    public class UserValidate
    {
        [Required(ErrorMessage = "Please Enter Email.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Password.")]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "Please Enter Confirm Password.")]
        [Compare("PasswordHash",ErrorMessage ="Password & Confirm Password does not match")]
        public string ConfirmPasswordHash { get; set; }
        [Required(ErrorMessage = "Please Enter Mobile Number.")]
        [Phone]
        public string mobile { get; set; }
        [Required(ErrorMessage = "Please Enter City.")]
        public string city { get; set; }
        [Required(ErrorMessage = "Please Enter Address.")]
        public string address { get; set; }
        [Required(ErrorMessage = "Please Enter First Name.")]
        public string firstname { get; set; }
        [Required(ErrorMessage = "Please Enter Last Name.")]
        public string lastname { get; set; }
        [Required(ErrorMessage = "Please Select State.")]
        public string state { get; set; }
        [Required(ErrorMessage = "Please Enter your cnic.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Enter Valid CNIC 13 characters")]
        public string cnic { get; set; }
        

    }
    [MetadataType(typeof(UserValidate))]
    public partial class AspNetUser
    {
        public string ConfirmPasswordHash { get; set; }
        public string RoleName { get; set; }
    }
}