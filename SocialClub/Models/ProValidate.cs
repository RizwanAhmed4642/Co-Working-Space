using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialClub.Models
{
    public class ProValidate
    {
    }
    [MetadataType(typeof(ProValidate))]
    public partial class PricePlan
    {
        public string ProductName { get; set; }
    }
}