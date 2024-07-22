using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialClub.Models
{
    public class Dashboard
    {
        public int TotalProducts { get; set; }
        public int TotalUsers { get; set; }
        public int TotalRevnue { get; set; }
        public int TotalReceivable { get; set; }
        public int TotalMessages { get; set; }
        public int TotalPackages { get; set; }
        public int TotalBookings { get; set; }
    }
}