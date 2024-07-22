using SocialClub.Configuration;
using SocialClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialClub.Controllers
{
    [AllowAnonymous]
    public class ContactUsController : Controller
    {
        protected SocialClubEntities db = new SocialClubEntities();
        private readonly AdminActivity_Logs Addlog = new AdminActivity_Logs();
        // GET: ContactUs
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(ContactDetail data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    data.CreatedDate = DateTime.Now;
                    data.ReadStatus = false;
                    db.ContactDetails.Add(data);
                    Addlog.AddLog("Save", "Visitor Submit Message In Application");
                    db.SaveChanges();
                    TempData["response"] = "Thank you for contact us, Administration will reach you soon !";
                    return RedirectToAction("Index", "ContactUs");
                }
                else
                {
                    TempData["response"] = "Invalid Model " + ModelState.Values;
                    return RedirectToAction("Index", "ContactUs");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Index", "ContactUs");

            }
        }
    }
}