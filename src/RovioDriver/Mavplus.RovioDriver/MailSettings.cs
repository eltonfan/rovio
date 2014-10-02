using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    public class MailSettings
    {
        public bool Enabled { get; set; }
        /// <summary>
        /// mail server address
        /// </summary>
        public string MailServer { get; set; }

        public int Port { get; set; }

        /// <summary>
        /// sender’s email address
        /// </summary>
        public string Sender { get; set; }
        /// <summary>
        /// receiver’s email address, multi-receivers separated by ‘;’
        /// </summary>
        public string Receiver { get; set; }
        /// <summary>
        /// subject of email
        /// </summary>
        public string Subject { get; set; }

        public string Body { get; set; }

        /// <summary>
        /// user name for logging into the MailServer
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// password for logging into the MailServer
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// whether the MailServer needs to check password
        /// </summary>
        public bool AuthRequired { get; set; }
        /// <summary>
        /// Ignored. iMilliSeconds
        /// </summary>
        public int Interval { get; set; }
    }
}
