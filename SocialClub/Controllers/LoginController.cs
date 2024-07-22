
using SocialClub.Configuration;
using SocialClub.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SocialClub.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly AdminActivity_Logs Addloginlog = new AdminActivity_Logs();
        protected SocialClubEntities db = new SocialClubEntities();
        protected EncryptDecrypt encry = new EncryptDecrypt();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AspNetUser data)
        {
            try
            {
                if (Membership.ValidateUser(data.Email, data.PasswordHash))
                {

                    FormsAuthentication.SetAuthCookie(data.Email, false);
                    var currentRole = Roles.GetRolesForUser();
                    if (data.Email == "admin@ab-sol.net" || User.IsInRole("AbsolAdmin") || User.IsInRole("Admin") || User.IsInRole("Administrator") || User.IsInRole("Manager") || User.IsInRole("Accountant"))
                    {
                        return RedirectToAction("Dashboard", "Administration");
                    }
                    else
                    {


                        return RedirectToAction("Index", "Home");

                    }
                }
                else
                {
                    TempData["response"] = "Invalid Email or Password";
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Login", "Login");
            }
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(AspNetUser data)
        {
            try
            {
                var isValid = db.AspNetUsers.ToList().Where(x => x.Email == data.Email).ToList();
                if (isValid.Count == 0)
                {
                    if (ModelState.IsValid)
                    {
                        data.is_active = true;
                        data.created_date = DateTime.Now;
                        data.UserName = data.firstname + " " + data.lastname;
                        data.PasswordHash = encry.Encrypt(data.PasswordHash);
                        data.ConfirmPasswordHash = data.PasswordHash;
                        var roles = db.AspNetRoles.ToList().Where(x => x.Name == "Customer").FirstOrDefault();
                        if (roles != null)
                        {
                            data.role_id = roles.Id;
                            var currentuser = db.AspNetUsers.Add(data);
                            db.SaveChanges();
                            AspNetUserRole userroles = new AspNetUserRole();
                            userroles.Roles_id = currentuser.role_id;
                            userroles.User_id = currentuser.Id;
                            db.AspNetUserRoles.Add(userroles);
                            db.SaveChanges();
                            TempData["response"] = "Thank You For Registration, You Can Login Now !";
                            return RedirectToAction("Login", "Login");
                        }
                        else
                        {
                            AspNetRole newrole = new AspNetRole();
                            newrole.Name = "Customer";
                            var currentRole = db.AspNetRoles.Add(newrole);
                            db.SaveChanges();
                            data.role_id = currentRole.Id;
                            var currentuser = db.AspNetUsers.Add(data);
                            db.SaveChanges();

                            AspNetUserRole userroles = new AspNetUserRole();
                            userroles.Roles_id = currentuser.role_id;
                            userroles.User_id = currentuser.Id;
                            db.AspNetUserRoles.Add(userroles);
                            db.SaveChanges();
                            TempData["response"] = "Thank You For Registration, You Can Login Now !";
                            return RedirectToAction("Login", "Login");
                        }
                    }
                    else
                    {
                        TempData["response"] = "Invalid Model " + ModelState.Values;
                        return RedirectToAction("Register", "Login");
                    }
                }
                else
                {
                    TempData["response"] = "Email Already Exists !";
                    return RedirectToAction("Register", "Login");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Register", "Login");

            }
        }

        public ActionResult Signout()
        {
            Addloginlog.AddLog("Logout", "Logout from Application");
            FormsAuthentication.SignOut();

            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Lost Password Function.
        /// Author: Rizwan Ahmed.
        /// </summary>
        /// <returns>View</returns>
        public ActionResult LostPassword()
        {
            return View();
        }
        /// <summary>
        /// Check User Exit then send Email to User For Rest Password
        ///  Author: Rizwan Ahmed.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        
        public ActionResult LostPassword(AspNetUser model)
        {
            try
            {
                var _user = db.AspNetUsers.Where(x => x.Email == model.Email).FirstOrDefault();
                if (_user != null)
                {
                    var mesg = "<div><h1>Reset Password</h1><br/><p>You have received this email to Reset Your Password. If you did not ask for a reset password, then kindly ignore this email or click to set new password.<a href='http://apps.ab-sol.net/vked/Login/ResetPassword' target='_blank'>  Reset Password </a> </p></div>";

                    EmailSend _emailsend = new EmailSend();

                    _emailsend.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

                    var _email = ConfigurationManager.AppSettings["MENAFATF_Email"];
                    var _pass = ConfigurationManager.AppSettings["From_Email_Password"];
                    var _dispName = "Absolute Social Club";
                    MailMessage mymessage = new MailMessage();
                    mymessage.To.Add(_user.Email);
                    mymessage.From = new MailAddress(_email, _dispName);
                    mymessage.Subject = "Reset Password";
                    mymessage.Body = mesg;
                    mymessage.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.EnableSsl = true;
                        smtp.Host = ConfigurationManager.AppSettings["host"];
                        smtp.Port = Int32.Parse(ConfigurationManager.AppSettings["SMTP_Port"]);
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential(_email, _pass);

                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                        smtp.Send(mymessage);

                    }




                    TempData["response"] = "Shortly you will received an email with reset link";
                    return RedirectToAction("LostPassword", "Login");
                }
                else
                {
                    TempData["resp"] = "Email Address Does Not Exist, Please Try again!";
                    return RedirectToAction("LostPassword", "Login");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("LostPassword", "Login");
            }
        }
        public ActionResult ResetPassword()
        {
            return View();
        }
        /// <summary>
        /// Reset Password function .its also send link on user Email.
        /// Auther :Rizwan Ahmed.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
       
        public ActionResult ResetPassword(PassValidate model)
        {
            try
            {
                
                var currentuser = db.AspNetUsers.ToList().Where(x => x.Email == model.Email).FirstOrDefault();
                if (currentuser != null)
                {

                    currentuser.PasswordHash = encry.Encrypt(model.PasswordHash);
                    currentuser.ConfirmPasswordHash = currentuser.PasswordHash;
                    db.Entry(currentuser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    var mesg = "<div><h1>Successfully !</h1><br/><p>Your Password has been Successfully Changed.Please click on this link to Login  <a href='http://apps.ab-sol.net/vked/Login/login' target='_blank'> Log in </a> </p></div>";

                    EmailSend _emailsend = new EmailSend();

                    _emailsend.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

                    var _email = ConfigurationManager.AppSettings["MENAFATF_Email"];
                    var _pass = ConfigurationManager.AppSettings["From_Email_Password"];
                    var _dispName = "Absolute Social Club";
                    MailMessage mymessage = new MailMessage();
                    mymessage.To.Add(model.Email);
                    mymessage.From = new MailAddress(_email, _dispName);
                    mymessage.Subject = "Reset Password";
                    mymessage.Body = mesg;
                    mymessage.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.EnableSsl = true;
                        smtp.Host = ConfigurationManager.AppSettings["host"];
                        smtp.Port = Int32.Parse(ConfigurationManager.AppSettings["SMTP_Port"]);
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential(_email, _pass);

                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                        smtp.Send(mymessage);

                    }
                    TempData["response"] = "Your Password has been Changed Successfully.";
                    return RedirectToAction("ResetPassword", "Login");
                }
                else
                {
                    TempData["resp"] = "Email Is Wrong.";
                    return RedirectToAction("ResetPassword", "Login");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Register", "Login");
            }
        }

    }
}