using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Mavplus.RovioDriver
{
    public class FirmwareVersion
    {
        /// <summary>
        /// Date and Time of the firmware.
        /// </summary>
        public DateTime DatePublished { get; private set; }
        /// <summary>
        /// $Id (Subversion) – filename, version number, date, time and author
        /// </summary>
        public string Version { get; private set; }

        static Regex regex = null;
        internal static FirmwareVersion Parse(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            if (regex == null)
            {//Jan 12 2010 14:41:24 $Revision: 5.3503$
                regex = new Regex(@"(?<time>[^\$]+)\$Revision: (?<rev>[^\$]+)\$",
                     RegexOptions.IgnoreCase | RegexOptions.Singleline);
            }
            Match match = regex.Match(str);
            if (!match.Success)
                return null;

            DateTime datePublished = DateTime.Parse(match.Groups["time"].Value);
            string version = match.Groups["rev"].Value;
            return new FirmwareVersion
            {
                DatePublished = datePublished,
                Version = version,
            };
        }
    }

    public class RovioVersionInfo
    {
        /// <summary>
        /// Date and Time of the firmware.
        /// </summary>
        public DateTime DatePublished { get; private set; }
        /// <summary>
        /// $Id (Subversion) – filename, version number, date, time and author
        /// </summary>
        public string Version { get; private set; }
        /// <summary>
        ///  LibNSVersion
        /// </summary>
        public string TrueTrackVersion { get; private set; }

        internal RovioVersionInfo(FirmwareVersion firmware, string trueTrackVersion)
        {
            if (firmware == null)
            {
                this.Version = "";
                this.DatePublished = new DateTime(1970, 1, 1);
            }
            else
            {
                this.Version = firmware.Version;
                this.DatePublished = firmware.DatePublished;
            }
            this.TrueTrackVersion = trueTrackVersion;
        }
    }
}
