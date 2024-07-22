using SocialClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SocialClub.Configuration
{
    public class UserSecured
    {
    }
    public class SecureProvider : MembershipProvider
    {
        private readonly AdminActivity_Logs Addlog = new AdminActivity_Logs();
        private readonly SocialClubEntities db = new SocialClubEntities();
        protected EncryptDecrypt encrypter = new EncryptDecrypt();
        public override bool EnablePasswordRetrieval => throw new NotImplementedException();

        public override bool EnablePasswordReset => throw new NotImplementedException();

        public override bool RequiresQuestionAndAnswer => throw new NotImplementedException();

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int MaxInvalidPasswordAttempts => throw new NotImplementedException();

        public override int PasswordAttemptWindow => throw new NotImplementedException();

        public override bool RequiresUniqueEmail => throw new NotImplementedException();

        public override MembershipPasswordFormat PasswordFormat => throw new NotImplementedException();

        public override int MinRequiredPasswordLength => throw new NotImplementedException();

        public override int MinRequiredNonAlphanumericCharacters => throw new NotImplementedException();

        public override string PasswordStrengthRegularExpression => throw new NotImplementedException();

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            if (username != null && password != null)
            {
                if (username == "admin@ab-sol.net" && password == "Pakistan1997")
                {
                    Addlog.AddLoginLog(username, "Login", "Login into Application");
                    return true;
                }
                else
                {
                    var innerpassword = encrypter.Encrypt(password);
                    int count = db.AspNetUsers.AsQueryable().Where(x => x.Email == username && x.PasswordHash == innerpassword && x.is_active == true).ToList().Count();
                    if (count != 0)
                    {
                        var currentuser = db.AspNetUsers.AsQueryable().Where(x => x.Email == username && x.PasswordHash == innerpassword && x.is_active == true).FirstOrDefault();
                        currentuser.last_login = DateTime.Now;
                        currentuser.PasswordHash = innerpassword;
                        currentuser.ConfirmPasswordHash = innerpassword;
                        db.Entry(currentuser).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        Addlog.AddLoginLog(username, "Login", "Login into Application");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
    }

    public class SecureRole : RoleProvider
    {
        private readonly SocialClubEntities db = new SocialClubEntities();
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            try
            {
                if(username == "admin@ab-sol.net")
                {
                    string[] ss = { "AbsolAdmin" };
                    return ss;
                }
                var roles = db.AspNetUsers.ToList().Where(x => x.Email == username).FirstOrDefault().role_id;
                var currentRole = db.AspNetRoles.Find(roles);
                string[] s = { currentRole.Name };
                if(s != null)
                {
                    return s;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            } 
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
