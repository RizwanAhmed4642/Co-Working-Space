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
    
    public partial class AspNetUserRole
    {
        public int id { get; set; }
        public Nullable<int> User_id { get; set; }
        public Nullable<int> Roles_id { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
    }
}