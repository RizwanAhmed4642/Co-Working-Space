using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialClub.Models
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public string Timeline { get; set; }
        [Required(ErrorMessage = "Please Enter Email.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Contact Number.")]
        [Range(0, 15, ErrorMessage = "Can only be between 0 .. 15")]
        public string Phone { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> PackageID { get; set; }
        public string PackageName { get; set; }
        public Nullable<System.DateTime> BookingDate { get; set; }
        public Nullable<bool> Status { get; set; }
        public string UserTitle { get; set; }
        public string Tax { get; set; }
        public string GrossAmount { get; set; }
        public string TotalSeats { get; set; }
        [Required(ErrorMessage = "Please Enter Name.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Enter Name.")]
        public string LastName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
       
        public Nullable<System.DateTime> PackageDuration { get; set; }
        [Required(ErrorMessage = "Select  Number Of people.")]
        public string NoOfPeople { get; set; }
    }
}