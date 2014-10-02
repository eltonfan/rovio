using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    public class NetworkConfig
    {
        public string SSID { get; set; }
        public string MACAddress { get; set; }
        public WifiMode Mode { get; set; }
        public string Key { get; set; }
        public int Channel { get; set; }
        public int WebPort { get; set; }

        /// <summary>
        /// true: Automatically from DHCP
        /// false: Manually
        /// </summary>
        public bool DHCPEnabled { get; set; }
        public string RovioIPAddress { get; set; }
        public string SubnetMask { get; set; }
        public string DefaultGateway { get; set; }
        public string DNS { get; set; }

        public string CurrentIP { get; set; }
        public string CurrentNetmask { get; set; }
        public string CurrentGateway { get; set; }
        public string CurrentDNS0 { get; set; }
    }
}
