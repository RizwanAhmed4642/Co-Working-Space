using SocialClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialClub.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        public ActionResult AdminLogs()
        {
            using (SocialClubEntities db = new SocialClubEntities())
            {
                List<Activity_Log> list = new List<Activity_Log>();
                List<Activity_Log> Adminloglist = db.Activity_Log.ToList<Activity_Log>();

                foreach (var item in Adminloglist)
                {
                    if (item.created_date != null)
                    {
                        item.Cdate = item.created_date.Value.ToString("MM/dd/yyyy");
                        list.Add(item);
                    }
                }

                return View(list);
            }
        }

     

    }
}
