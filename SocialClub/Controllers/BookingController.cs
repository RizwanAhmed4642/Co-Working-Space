using SocialClub.Configuration;
using SocialClub.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace SocialClub.Controllers
{
    public class BookingController : Controller
    {
        private readonly AdminActivity_Logs Addlog = new AdminActivity_Logs();
        protected SocialClubEntities db = new SocialClubEntities();
        public JavaScriptSerializer sr = new JavaScriptSerializer();
        // GET: Booking
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Add Booking Method .
        /// Author: Rizwan Ahmed.
        /// <param name="id"></param>
        /// <param name="timeline"></param>
        /// <param name="grossamount"></param>
        /// <param name="PlanName"></param>
        /// <param name="PlanId"></param>
        /// <returns></returns>
        /// </summary>
        public ActionResult BookingMe(int id, string timeline, string grossamount, string PlanName, string PlanId)
        {
            BookingViewModel _resultmodel = new BookingViewModel();
            var Product = db.Products.Find(id);
            //var Plan = db.PricePlans.Where(x => x.PlanName == PlanName);
            int packageId = Convert.ToInt32(PlanId);
            _resultmodel.ProductID = id;
            _resultmodel.Timeline = timeline;
            _resultmodel.PackageName = PlanName;
            _resultmodel.PackageID = packageId;
            _resultmodel.GrossAmount = grossamount;
            _resultmodel.GrossAmount = grossamount;
            ViewBag.ProductName = Product.ProductName;
            _resultmodel.ProductName = Product.ProductName;
            //ViewBag.TimeLine = new SelectList(db.PricePlans.ToList(), "PlanID", "TimeLine");
            ViewBag.Plan = new SelectList(db.PricePlans.ToList(), "PlanID", "PlanName");
            return View(_resultmodel);
        }
        [HttpPost]
        public ActionResult BookingMe(BookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Booking _entity = new Booking();
                    var email = User.Identity.Name.ToString();

                    _entity.PackageName = model.PackageName;

                    _entity.Email = model.Email;
                    _entity.Phone = model.Phone;
                    _entity.ProductID = model.ProductID;
                    _entity.PackageID = model.PackageID;
                    _entity.BookingDate = DateTime.Now;
                    _entity.Status = null;
                    _entity.UserTitle = model.UserTitle;
                    _entity.FirstName = model.LastName;
                    _entity.LastName = model.LastName;
                    _entity.StartDate = model.StartDate;
                    _entity.PackageDuration = model.PackageDuration;
                    _entity.NoOfPeople = model.NoOfPeople;
                    _entity.ProductName = model.ProductName;
                    _entity.GrossAmount = model.GrossAmount;
                    _entity.Timeline = model.Timeline;



                    model.BookingDate = DateTime.Now;
                    model.Status = false;
                    if (model.Timeline == "Month")
                    {
                        var _durationdate = model.StartDate.Value.AddDays(30);
                        _entity.PackageDuration = _durationdate;

                    }
                    else if (model.Timeline == "Month")
                    {
                        var _durationdate = model.StartDate.Value.AddDays(7);
                        _entity.PackageDuration = _durationdate;

                    }
                    else if (model.Timeline == "day")
                    {
                        var _durationdate = model.StartDate.Value.AddDays(1);
                        _entity.PackageDuration = _durationdate;

                    }
                    db.Bookings.Add(_entity);
                    db.SaveChanges();

                    var mesg = "<div><h1>Thank you !</h1><br/><p>Thank you for your booking request as well as for your interest in our Company. Since we have received great response, we need a little time to consider all booking requests.Therefore, we ask you to be patient until we meet again with you.</p></div>";
                    ///  when user done a booking here send an email to 'administration' about the booking request 
                    EmailSend _emailsend = new EmailSend();
                    //_emailsend.Message = "You have successfully Applied for Booking";
                    _emailsend.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

                    var _email = ConfigurationManager.AppSettings["MENAFATF_Email"];
                    var _pass = ConfigurationManager.AppSettings["From_Email_Password"];
                    var _dispName = "Absolute Social Club";
                    MailMessage mymessage = new MailMessage();
                    mymessage.To.Add(model.Email);
                    mymessage.From = new MailAddress(_email, _dispName);
                    mymessage.Subject = "Booking Request";
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


                    ///Sending Email to Admin For booking Request
                    var _msgForAdmin = "<div><h3>Dear Admin,</h3><br/><p>We have received a booking request from " + "<b>" + _entity.UserTitle + " " + _entity.FirstName + " " + _entity.LastName + "</b>" + " " + "Email :" + _entity.Email + "," + " " + "requesting for a slot starting from" + " " + "<b>" + _entity.StartDate.Value.ToString("dd/MMM/yyyy") + "</b>" + " " + "till" + " " + "<b>" + _entity.PackageDuration.Value.ToString("dd/MMM/yyyy") + "</b>" + " " + "If there is a requested slot available, kindly approve the request." + "</p></div>";
                    ///  when user done a booking here send an email to 'administration' about the booking request 
                    EmailSend _emaiSendForAdmin = new EmailSend();
                    //_emailsend.Message = "You have successfully Applied for Booking";
                    _emaiSendForAdmin.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

                    var _emailAdmin = ConfigurationManager.AppSettings["MENAFATF_Email"];
                    var _password = ConfigurationManager.AppSettings["From_Email_Password"];
                    var _disputeName = "Absolute Social Club";
                    MailMessage mymessageAdmin = new MailMessage();
                    mymessageAdmin.To.Add("rizwanahmed4642@gmail.com");
                    mymessageAdmin.From = new MailAddress(_email, _disputeName);
                    mymessageAdmin.Subject = "Booking Request";
                    mymessageAdmin.Body = _msgForAdmin;
                    mymessageAdmin.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.EnableSsl = true;
                        smtp.Host = ConfigurationManager.AppSettings["host"];
                        smtp.Port = Int32.Parse(ConfigurationManager.AppSettings["SMTP_Port"]);
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential(_emailAdmin, _password);

                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                        smtp.Send(mymessageAdmin);

                    }
                    // TempData["response"] = "Plan Added sucefully";
                    return RedirectToAction("Details", "Home", new { id = model.ProductID });
                }
                catch (Exception ex)
                {

                    var massage = ex.Message;
                }
            }

            ViewBag.Plan = new SelectList(db.PricePlans.ToList(), "PlanID", "PlanName");
            return View();
        }
        /// <summary>
        /// List of All Bookings.
        /// Author: Rizwan Ahmed.
        /// </summary>
        /// <returns>_resultModel</returns>
        public ActionResult ListBooking()
        {
            var _resultModel = new List<Booking>();
            try
            {
                var _bookinglist = db.Bookings.ToList();


                foreach (var item in _bookinglist)
                {
                    _resultModel.Add(item);

                }

                return View(_resultModel);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ListBooking", "Booking");
            }
        }
        /// <summary>
        /// Approved Bookings Function.
        /// Author: Rizwan Ahmed.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ListBooking</returns>
        public ActionResult ApprovedBooking(int id)

        {
            var _booking = db.Bookings.Find(id);
            _booking.Status = true;
            db.Entry(_booking).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            ///sending Email to Customer

            var mesg = "<div><h1>Approved Booking</h1><br/><p>We are delighted to inform you that your booking request has been approved. We look forward for you to choose us next time also.</p><p> Your account and payment invoice has generated. Please login from the following link <a href='www.absolute-social.com' target='_blank'> Social Club </a><h5> Login Credential are</h5><h4> UserName:" + _booking.Email + "</h4><h4> Password: Your Cnic</h4></p></div>";

            EmailSend _emailsend = new EmailSend();

            _emailsend.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

            var _email = ConfigurationManager.AppSettings["MENAFATF_Email"];
            var _pass = ConfigurationManager.AppSettings["From_Email_Password"];
            var _dispName = "Absolute Social Club";
            MailMessage mymessage = new MailMessage();
            mymessage.To.Add(_booking.Email);
            mymessage.From = new MailAddress(_email, _dispName);
            mymessage.Subject = "Approved Booking";
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

            return RedirectToAction("ListBooking");
        }
        /// <summary>
        /// Rejected Booking function.
        /// Author: Rizwan Ahmed.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ListBooking</returns>
        public ActionResult RejectedBooking(int id)
        {
            var _booking = db.Bookings.Find(id);
            _booking.Status = false;
            db.Entry(_booking).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            var mesg = "<div><h1>Reject Booking</h1><br/><p>Your booking has been rejected.</p></div>";

            EmailSend _emailsend = new EmailSend();

            _emailsend.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

            var _email = ConfigurationManager.AppSettings["MENAFATF_Email"];
            var _pass = ConfigurationManager.AppSettings["From_Email_Password"];
            var _dispName = "Absolute Social Club";
            MailMessage mymessage = new MailMessage();
            mymessage.To.Add(_booking.Email);
            mymessage.From = new MailAddress(_email, _dispName);
            mymessage.Subject = "Rejected Booking";
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
            return RedirectToAction("ListBooking");
        }


    }
}