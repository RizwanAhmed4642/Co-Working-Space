using SocialClub.Configuration;
using SocialClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialClub.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        protected SocialClubEntities db = new SocialClubEntities();
        protected EncryptDecrypt encry = new EncryptDecrypt();
        public ActionResult MyProfile()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(PassValidate data)
        {
            try
            {
                var oldpass = encry.Encrypt(data.CurrentPasswordHash);
                var currentuser = db.AspNetUsers.ToList().Where(x => x.Email == User.Identity.Name && x.PasswordHash == oldpass).FirstOrDefault();
                if(currentuser != null)
                {

                    currentuser.PasswordHash = encry.Encrypt(data.PasswordHash);
                    currentuser.ConfirmPasswordHash = currentuser.PasswordHash;
                    db.Entry(currentuser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["response"] = "Password Changed Successfully.";
                    return RedirectToAction("ChangePassword", "Profile");
                }
                else
                {
                    TempData["response"] = "Old Password Is Wrong.";
                    return RedirectToAction("ChangePassword", "Profile");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Register", "Login");
            }
            return View();
        }

    }
}