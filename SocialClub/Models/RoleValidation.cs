using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialClub.Models
{
    public class RoleValidation
    {
        [Required(ErrorMessage ="Please Enter Role Name.")]
        public string Name { get; set; }
    }
    [MetadataType(typeof(RoleValidation))]
    public partial class AspNetRole
    {
    }
}