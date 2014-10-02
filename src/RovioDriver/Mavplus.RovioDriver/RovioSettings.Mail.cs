using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    partial class RovioSettings
    {
        public class MailGroup : SettingGroup
        {
            bool enabled = false;
            /// <summary>
            /// 
            /// </summary>
            public bool Enabled
            {
                get { return this.enabled; }
                set
                {
                    if (this.enabled == value)
                        return;
                    this.enabled = value;

                    this.modified = true;
                }
            }


            string mailServer = "";
            /// <summary>
            /// mail server address
            /// </summary>
            public string MailServer
            {
                get { return this.mailServer; }
                set
                {
                    if (this.mailServer == value)
                        return;
                    this.mailServer = value;

                    this.modified = true;
                }
            }

            int port = 25;
            /// <summary>
            /// 
            /// </summary>
            public int Port
            {
                get { return this.port; }
                set
                {
                    if (this.port == value)
                        return;
                    this.port = value;

                    this.modified = true;
                }
            }

            string sender = "";
            /// <summary>
            /// sender’s email address
            /// </summary>
            public string Sender
            {
                get { return this.sender; }
                set
                {
                    if (this.sender == value)
                        return;
                    this.sender = value;

                    this.modified = true;
                }
            }

            string receiver = "";
            /// <summary>
            /// receiver’s email address, multi-receivers separated by ‘;’
            /// </summary>
            public string Receiver
            {
                get { return this.receiver; }
                set
                {
                    if (this.receiver == value)
                        return;
                    this.receiver = value;

                    this.modified = true;
                }
            }

            string subject = "";
            /// <summary>
            /// subject of email
            /// </summary>
            public string Subject
            {
                get { return this.subject; }
                set
                {
                    if (this.subject == value)
                        return;
                    this.subject = value;

                    this.modified = true;
                }
            }

            string body = "";
            /// <summary>
            /// 邮件正文。
            /// </summary>
            public string Body
            {
                get { return this.body; }
                set
                {
                    if (this.body == value)
                        return;
                    this.body = value;

                    this.modified = true;
                }
            }

            string userName = "";
            /// <summary>
            /// user name for logging into the MailServer
            /// </summary>
            public string UserName
            {
                get { return this.userName; }
                set
                {
                    if (this.userName == value)
                        return;
                    this.userName = value;

                    this.modified = true;
                }
            }

            string password = "";
            /// <summary>
            /// password for logging into the MailServer
            /// </summary>
            public string Password
            {
                get { return this.password; }
                set
                {
                    if (this.password == value)
                        return;
                    this.password = value;

                    this.modified = true;
                }
            }

            bool authRequired = true;
            /// <summary>
            /// whether the MailServer needs to check password
            /// </summary>
            public bool AuthRequired
            {
                get { return this.authRequired; }
                set
                {
                    if (this.authRequired == value)
                        return;
                    this.authRequired = value;

                    this.modified = true;
                }
            }

            int interval = 1000;
            /// <summary>
            /// Ignored. iMilliSeconds
            /// </summary>
            public int Interval
            {
                get { return this.interval; }
                set
                {
                    if (this.interval == value)
                        return;
                    this.interval = value;

                    this.modified = true;
                }
            }
            
            readonly RovioSettings owner = null;
            internal MailGroup(RovioSettings owner)
            {
                this.owner = owner;
            }

            internal override void Load(Dictionary<FlashParameters, Int32> flashParameters, RovioStatusReport report, RovioMcuReport mcuReport)
            {
                MailSettings settings = owner.rovio.API.GetMail();
                this.enabled = settings.Enabled;
                this.mailServer = settings.MailServer;
                this.port = settings.Port;
                this.sender = settings.Sender;
                this.receiver = settings.Receiver;
                this.subject = settings.Subject;
                this.body = settings.Body;
                this.userName = settings.UserName;
                this.password = settings.Password;
                this.authRequired = settings.AuthRequired;
                this.interval = settings.Interval;

                this.modified = false;
            }

            public override void Save()
            {
                if (!this.modified)
                    return;

                MailSettings settings = new MailSettings();
                settings.Enabled = this.enabled;
                settings.MailServer = this.mailServer;
                settings.Port = this.port;
                settings.Sender = this.sender;
                settings.Receiver = this.receiver;
                settings.Subject = this.subject;
                settings.Body = this.body;
                settings.UserName = this.userName;
                settings.Password = this.password;
                settings.AuthRequired = this.authRequired;
                settings.Interval = this.interval;

                owner.rovio.API.SetMail(settings);

                this.modified = false;
            }
        }
    }
}
