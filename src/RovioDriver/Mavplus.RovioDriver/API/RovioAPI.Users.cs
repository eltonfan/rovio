using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver.API
{

    partial class RovioAPI
    {
        /// <summary>
        /// Get the username who sent this HTTP request.
        /// </summary>
        /// <param name="ShowPrivilege"></param>
        /// <returns>Privilege = 0 (for common user),Privilege = 1 (for super user),(Always returns 0 if it is in Non-authorization mode under SetUserCheck.cgi)</returns>
        public UserInformation GetMyself(bool ShowPrivilege)
        {
            RovioResponse response = this.Request("/GetMyself.cgi",
                new RequestItem("ShowPrivilege", ShowPrivilege ? 1 : 0));
            
            UserGroups group = UserGroups.Unknown;
            if(response.ContainsKey("Privilege"))
            {
                if(response["Privilege"] == "1")
                    group = UserGroups.Administrator;
                else if(response["Privilege"] == "0")
                    group = UserGroups.User;
                else
                    group = UserGroups.Unknown;
            }

            UserInformation info = new UserInformation(
                group, response["Name"]);
            return info;
        }


        /// <summary>
        /// Add a user or change the password for existed user.
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Pass"></param>
        /// <returns></returns>
        public bool SetUser(string User, string Pass)
        {
            RovioResponse response = this.Request("/SetUser.cgi",
                new RequestItem("User", User),
                new RequestItem("Pass", Pass));

            return true;
        }

        /// <summary>
        /// Delete a user account.
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public bool DelUser(string User)
        {
            RovioResponse response = this.Request("/DelUser.cgi",
                new RequestItem("User", User));

            return true;
        }

        /// <summary>
        /// Get the users list of IP Camera.
        /// </summary>
        /// <param name="ShowPrivilege"></param>
        /// <returns></returns>
        public string GetUser(bool ShowPrivilege)
        {
            RovioResponse response = this.Request("/GetUser.cgi",
                new RequestItem("ShowPrivilege", ShowPrivilege));

            return "";
        }

        /// <summary>
        /// Enable or disable user authorization check.
        /// </summary>
        /// <param name="Check"></param>
        /// <returns></returns>
        public void SetUserCheck(bool Check)
        {
            RovioResponse response = this.Request("/SetUserCheck.cgi",
                new RequestItem("Check", Check));
        }
    }
}
