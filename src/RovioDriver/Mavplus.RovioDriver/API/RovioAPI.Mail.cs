using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver.API
{
    partial class RovioAPI
    {
        /// <summary>
        /// Configure email for sending IPCam images.
        /// </summary>
        public void SetMail(MailSettings settings)
        {
            RovioResponse response = this.Request("/SetMail.cgi",
                new RequestItem("Enable", settings.Enabled ? "true" : "false"),
                new RequestItem("MailServer", settings.MailServer),
                new RequestItem("Port", settings.Port),
                new RequestItem("Sender", settings.Sender),
                new RequestItem("Receiver", settings.Receiver),
                new RequestItem("Subject", settings.Subject),
                new RequestItem("Body", settings.Body),
                new RequestItem("User", settings.UserName),
                new RequestItem("PassWord", settings.Password),
                new RequestItem("CheckFlag", settings.AuthRequired ? "CHECK" : ""),
                new RequestItem("Interval", settings.Interval));
        }

        /// <summary>
        /// Get email settings.
        /// </summary>
        /// <returns></returns>
        public MailSettings GetMail()
        {
            RovioResponse response = this.Request("/GetMail.cgi");

            MailSettings settings = new MailSettings();
            settings.MailServer = response["MailServer"];
            settings.Port = int.Parse(response["Port"]);
            settings.Sender = response["Sender"];
            settings.Receiver = response["Receiver"];
            settings.Subject = response["Subject"];
            settings.Body = response["Body"];
            settings.UserName = response["User"];
            settings.Password = response["PassWord"];
            settings.AuthRequired = RovioAPI.GetBoolean(int.Parse(response["CheckFlag"]));

            settings.Enabled = RovioAPI.GetBoolean(int.Parse(response["Enable"]));

            return settings;
        }

        /// <summary>
        /// Send an email with IPCam images.
        /// </summary>
        /// <returns></returns>
        public string SendMail()
        {
            RovioResponse response = this.Request("/SendMail.cgi");
            return "";
        }
    }
}
