using SocialClub.Configuration;
using SocialClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialClub.Controllers
{
    [Authorize(Roles = "Admin, Administrator, AbsolAdmin")]
    public class AdministrationController : Controller
    {
        private readonly AdminActivity_Logs Addlog = new AdminActivity_Logs();
        protected SocialClubEntities db = new SocialClubEntities();
        protected EncryptDecrypt encry = new EncryptDecrypt();

        public ActionResult Dashboard()
        {
            Dashboard dash = new Dashboard();
            int usercounter = db.AspNetUsers.ToList().Where(x => x.is_active == true).Count();
            dash.TotalUsers = usercounter;
            var productscounter = db.Products.ToList().Where(x => x.Status == true).Count();
            dash.TotalProducts = productscounter;
            var packagecounter = db.PricePlans.ToList().Where(x => x.Status == true).Count();
            dash.TotalPackages = packagecounter;
            var messagescounter = db.ContactDetails.ToList().Where(x => x.ReadStatus == false).Count();
            dash.TotalMessages = messagescounter;
            return View(dash);
        }
        public ActionResult Roles()
        {
            return View();
        }            
        [HttpPost]
        public ActionResult AddRoles(AspNetRole data)
        {
            try
            {
                var alldata = db.AspNetRoles.ToList().Exists(x => x.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase));
                if(alldata == false)
                {
                    if (ModelState.IsValid)
                    {
                        db.AspNetRoles.Add(data);
                        Addlog.AddLog("Save", "Save Role In Application");
                        db.SaveChanges();
                        TempData["response"] = "Role Added Successfully !";
                        return RedirectToAction("ListRoles", "Administration");
                    }
                    else
                    {
                        TempData["response"] = "Invalid Model " + ModelState.Values;
                        return RedirectToAction("Roles", "Administration");
                    }
                }
                else
                {
                    TempData["response"] = "Same Record Already Exists !";
                    return RedirectToAction("Roles", "Administration");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Roles", "Administration");

            }
        }

        public ActionResult ListRoles()
        {
            try
            {
                var data = db.AspNetRoles.AsQueryable().ToList();
                return View(data);
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Roles", "Administration");
            }
        }

        public ActionResult DeleteRoles(int id)
        {
            try
            {
                var data = db.AspNetRoles.Find(id);
                if(data != null)
                {
                    db.AspNetRoles.Remove(data);
                    Addlog.AddLog("Delete", "Delete Role from Application");
                    db.SaveChanges();
                    TempData["response"] = "Role Deleted Successfully !";
                    return RedirectToAction("ListRoles", "Administration");
                }
                else
                {
                    TempData["response"] = "Role Delete Failed !";
                    return RedirectToAction("ListRoles", "Administration");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Roles", "Administration");
            }
        }

        public ActionResult UpdateRoles(int id)
        {
            try
            {
                var data = db.AspNetRoles.Find(id);
                if (data != null)
                {
                    return View(data);
                }
                else
                {
                    TempData["response"] = "Role Not Found !";
                    return RedirectToAction("ListRoles", "Administration");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Roles", "Administration");
            }
        }

        [HttpPost]
        public ActionResult UpdateRoles(AspNetRole data, int id)
        {
            try
            {
                var alldata = db.AspNetRoles.ToList().Exists(x => x.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != id);
                if (alldata == false)
                {
                    var record = db.AspNetRoles.Find(id);
                    if (record != null)
                    {
                        record.Name = data.Name;
                        db.Entry(record).State = System.Data.Entity.EntityState.Modified;

                        db.SaveChanges();
                        Addlog.AddLog("Update", "Update Role in Application");
                        TempData["response"] = "Role Updated Successfully !";
                        return RedirectToAction("ListRoles", "Administration");
                    }
                    else
                    {
                        TempData["response"] = "Role Update Failed !";
                        return RedirectToAction("ListRoles", "Administration");
                    }
                }
                else
                {
                    TempData["response"] = "Same Record Already Exists !";
                    return RedirectToAction("UpdateRoles", "Administration");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Roles", "Administration");
            }
        }
        public ActionResult ListUsers()
        {
            var allusers = db.AspNetUsers.ToList();
            var allrole = db.AspNetRoles.ToList();
            if(allusers.Count > 0)
            {
                for (int i = 0; i < allusers.Count(); i++)
                {
                    foreach (var item in allrole.ToList().Where(x=>x.Id == allusers[i].role_id))
                    {
                        allusers[i].RoleName = item.Name;
                    }
                }
            }
            return View(allusers);
        }
       

        public ActionResult UserDetails(int id)
        {
            var allusers = db.AspNetUsers.ToList().Where(x=>x.Id == id).ToList();
            var allrole = db.AspNetRoles.ToList();
            if (allusers.Count > 0)
            {
                for (int i = 0; i < allusers.Count(); i++)
                {
                    foreach (var item in allrole.ToList().Where(x => x.Id == allusers[i].role_id))
                    {
                        allusers[i].RoleName = item.Name;
                    }
                }
            }
            return View(allusers.FirstOrDefault());
        }

        public ActionResult DeleteUser(int id)
        {
            try
            {
                var data = db.AspNetUsers.Find(id);
                if (data != null)
                {
                    db.AspNetUsers.Remove(data);
                    db.SaveChanges();
                    Addlog.AddLog("Delete", "Delete User from Application");
                    TempData["response"] = "User Deleted Successfully !";
                    return RedirectToAction("ListUsers", "Administration");
                }
                else
                {
                    TempData["response"] = "User Delete Failed !";
                    return RedirectToAction("ListUsers", "Administration");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ListUsers", "Administration");
            }
        }

        public ActionResult RegisterUser()
        {
            var data = db.AspNetRoles.ToList();
            ViewBag.role_id = new SelectList(data.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(AspNetUser data)
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
                            data.role_id = data.role_id;
                            var currentuser = db.AspNetUsers.Add(data);
                            db.SaveChanges();
                            AspNetUserRole userroles = new AspNetUserRole();
                            userroles.Roles_id = currentuser.role_id;
                            userroles.User_id = currentuser.Id;
                            db.AspNetUserRoles.Add(userroles);
                           Addlog.AddLog("Save", "Save User in Application");
                              db.SaveChanges();
                            TempData["response"] = "User Registered Successfully.";
                            return RedirectToAction("ListUsers", "Administration");
                    }
                    else
                    {
                        TempData["response"] = "Invalid Model " + ModelState.Values;
                        return RedirectToAction("RegisterUser", "Administration");
                    }
                }
                else
                {
                    TempData["response"] = "Email Already Exists !";
                    return RedirectToAction("RegisterUser", "Administration");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("RegisterUser", "Administration");

            }
        }

        public ActionResult ListVisitorMessages()
        {
            var allmessages = db.ContactDetails.ToList();
            return View(allmessages);
        }
        public ActionResult DeleteVisitorMessage(int id)
        {
            try
            {
                var data = db.ContactDetails.Find(id);
                if (data != null)
                {
                    db.SaveChanges();
                    Addlog.AddLog("Delete", "Delete Visitor Message from Application");
                    TempData["response"] = "Message Deleted Successfully !";
                    return RedirectToAction("ListVisitorMessages", "Administration");
                }
                else
                {
                    TempData["response"] = "Message Delete Failed !";
                    return RedirectToAction("ListVisitorMessages", "Administration");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ListVisitorMessages", "Administration");
            }
        }

        public ActionResult ReadVisitorMessage(int id)
        {
            try
            {
                var data = db.ContactDetails.Find(id);
                if (data != null)
                {
                    data.ReadStatus = true;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ListVisitorMessages", "Administration");
                }
                else
                {
                    return RedirectToAction("ListVisitorMessages", "Administration");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ListVisitorMessages", "Administration");
            }
        }
    }
}