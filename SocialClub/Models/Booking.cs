//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SocialClub.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Booking
    {
        public int Id { get; set; }
        public string Timeline { get; set; }
        public string Email { get; set; }
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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> PackageDuration { get; set; }
        public string NoOfPeople { get; set; }
        public Nullable<bool> CancleRequest { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> CRDate { get; set; }
        public Nullable<bool> CAStatus { get; set; }
        public Nullable<bool> CRStatus { get; set; }
        public Nullable<System.DateTime> CARDate { get; set; }
        public string CARBy { get; set; }
    }
}
