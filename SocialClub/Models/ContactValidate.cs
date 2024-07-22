using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialClub.Models
{
    public class ContactValidate
    {
        [Required(ErrorMessage = "Please Enter FullName.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Please Enter Email.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Mobile Number.")]
        [Phone]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Please Enter Subject.")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Please Enter Message.")]
        public string Message { get; set; }
    }
    [MetadataType(typeof(ContactValidate))]
    public partial class ContactDetail
    {
    }
}