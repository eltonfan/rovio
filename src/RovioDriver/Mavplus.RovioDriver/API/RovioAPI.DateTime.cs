using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver.API
{
    partial class RovioAPI
    {
        static TimeZoneInfo CreateTimeZone(TimeSpan baseUtcOffset)
        {
            if (TimeZoneInfo.Local.BaseUtcOffset == baseUtcOffset)
            {//如果是本地时区，则无需寻找
                return TimeZoneInfo.Local;
            }
            //查找
            foreach (TimeZoneInfo item in TimeZoneInfo.GetSystemTimeZones())
            {
                if (item.BaseUtcOffset == baseUtcOffset)
                    return item;
            }
            //未找到系统定义的，则新建
            return TimeZoneInfo.CreateCustomTimeZone("Custom", baseUtcOffset, "自定义时区", "自定义时区");
        }

        /// <summary>
        /// Set server time zone and time.
        /// </summary>
        /// <param name="Sec1970">seconds since "00:00:00 1/1/1970".</param>
        /// <param name="TimeZone">Time zone in minutes. (e.g. Beijing is GMT+08:00, TimeZone = -480)</param>
        /// <returns></returns>
        public string SetTime(DateTime time, TimeZoneInfo timeZone)
        {
            DateTime date1970 = new DateTime(1970, 1, 1);
            long sec1970 = (long)time.Subtract(date1970).TotalSeconds;

            RovioResponse response = this.Request("/SetTime.cgi",
                new RequestItem("Sec1970", sec1970),
                new RequestItem("TimeZone", 0 - (int)timeZone.BaseUtcOffset.TotalMinutes));
            return "";
        }

        /// <summary>
        /// Get current IP Camera's time zone and time.
        /// </summary>
        /// <returns></returns>
        void GetTime(out DateTime timeUtc, out TimeZoneInfo timeZone, out string ntpServer, out bool useNtp)
        {
            ///GetTime.cgi[?JsVar=variable[&OnJs=function]]
            timeUtc = DateTime.MinValue;
            timeZone = TimeZoneInfo.Local;
            ntpServer = "";
            useNtp = false;

            RovioResponse dic = this.Request("/GetTime.cgi");
            if (dic == null)
                return;
            long sec1970;
            if (!(dic.ContainsKey("Sec1970") && long.TryParse(dic["Sec1970"], out sec1970)))
                return;
            int timeZoneMinutes;
            if (!(dic.ContainsKey("TimeZone") && int.TryParse(dic["TimeZone"], out timeZoneMinutes)))
                return;

            DateTime date1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            timeUtc = date1970.AddSeconds(sec1970);
            timeZone = CreateTimeZone(TimeSpan.FromMinutes(-timeZoneMinutes));

            ntpServer = dic.ContainsKey("NtpServer") ? dic["NtpServer"] : "";
            useNtp = dic.ContainsKey("UseNtp") ? (dic["UseNtp"] == "1") : false;
        }

        public DateTime GetTime()
        {
            DateTime timeUtc;
            TimeZoneInfo timeZone;
            string ntpServer;
            bool useNtp;
            GetTime(out timeUtc, out timeZone, out ntpServer, out useNtp);
            return TimeZoneInfo.ConvertTimeFromUtc(timeUtc, timeZone);
        }
    }
}
