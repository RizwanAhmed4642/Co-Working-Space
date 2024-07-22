using SocialClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialClub.Configuration
{
    public class AdminActivity_Logs
    {
        //  private readonly seed4meEntities db = new seed4meEntities();
        protected Activity_Log log = null;
       public AdminActivity_Logs()
        {
            log = new Activity_Log();
        }

        public void AddLog(string operation_name = "", string message = "")
        {     SocialClubEntities db = new SocialClubEntities();

        var count = db.Activity_Log.ToList().Count();
                var userNameis = HttpContext.Current.User.Identity.Name;
                var completemessage =message;
                log.created_by = userNameis;
                log.created_date = DateTime.Now;
                log.new_details = completemessage;
                log.operation_name = operation_name;
                log.log_time = (count + 1).ToString();
                db.Activity_Log.Add(log);
                db.SaveChanges();                
        }

        public void AddLoginLog(string username="",string operation_name = "", string message = "")
        {
            SocialClubEntities db = new SocialClubEntities();

            var count = db.Activity_Log.ToList().Count();

            var userNameis = username;
            var completemessage = message;

            log.created_by = userNameis;
            log.created_date = DateTime.Now;
            log.new_details = completemessage;
            log.operation_name = operation_name;
            log.log_time = (count + 1).ToString();
            db.Activity_Log.Add(log);
            db.SaveChanges();
        }

    }

}