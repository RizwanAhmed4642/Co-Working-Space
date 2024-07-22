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
    
    public partial class PricePlan
    {
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public string PriceRange { get; set; }
        public string TimeRange { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int ProductID { get; set; }
        public string FixedTime { get; set; }
        public string FixedPrice { get; set; }
        public Nullable<bool> IsPriceFixed { get; set; }
        public Nullable<bool> IsTimeFixed { get; set; }
        public string Timeline { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
