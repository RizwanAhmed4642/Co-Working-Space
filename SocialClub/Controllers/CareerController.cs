using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialClub.Controllers
{
    [Authorize]
    public class CareerController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult PostCareer()
        {
            return View();
        }
    }
}