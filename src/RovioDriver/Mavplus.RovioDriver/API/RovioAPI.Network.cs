using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver.API
{
    public class WifiInfo
    {
        public string ESSID { get; private set; }
        public WifiMode Mode { get; private set; }
        /// <summary>
        /// true: Security-enabled, false: Unsecure.
        /// </summary>
        public bool Encode { get; private set; }
        /// <summary>
        /// strength  0 - 100
        /// </summary>
        public int Quality { get; private set; }

        public WifiInfo(string ssid, WifiMode mode, bool encode, int quality)
        {
            this.ESSID = ssid;
            this.Mode = mode;
            this.Encode = encode;
            this.Quality = quality;
        }
    }
    partial class RovioAPI
    {
        /// <summary>
        /// 扫描Wifi网络。
        /// </summary>
        /// <returns></returns>
        public WifiInfo[] ScanWlan()
        {
            //ESSID = 2#3004
            //Mode = Managed
            //Encode = 1
            //Quality = 93
            RovioResponse response = this.Request("/ScanWlan.cgi");
            
            List<string> listSSID = new List<string>();
            List<WifiMode> listMode = new List<WifiMode>();
            List<bool> listEncode = new List<bool>();
            List<int> listQuality = new List<int>();
            foreach (RovioResponseItem item in response)
            {
                switch(item.Key)
                {
                    case "ESSID":
                        listSSID.Add(item.Value);
                        break;
                    case "Mode":
                        WifiMode mode;
                        if (item.Value == "Managed")
                            mode = WifiMode.WirelessNetwork;
                        else
                            mode = WifiMode.Computer2ComputerNetwork;
                        listMode.Add(mode);
                        break;
                    case "Encode":
                        listEncode.Add((item.Value == "1"));
                        break;
                    case "Quality":
                        listQuality.Add(int.Parse(item.Value));
                        break;
                }
            }
            List<WifiInfo> listWifi = new List<WifiInfo>();
            for (int i = 0; i < listSSID.Count; i++)
            {
                listWifi.Add(new WifiInfo(
                    (i > listSSID.Count - 1) ? "" : listSSID[i],
                    (i > listMode.Count - 1) ? WifiMode.WirelessNetwork : listMode[i],
                    (i > listEncode.Count - 1) ? false : listEncode[i],
                    (i > listQuality.Count - 1) ? 0 : listQuality[i]));
            }

            return listWifi.ToArray();
        }

        public NetworkConfig GetNetworkConfig()
        {
            RovioResponse response = this.Request("/Cmd.cgi",
                new RequestItem("Cmd", "GetWlan.cgi"),
                new RequestItem("Cmd", "GetIP.cgi"),
                new RequestItem("Cmd", "GetMac.cgi"),
                new RequestItem("Cmd", "GetHttp.cgi"));
            NetworkConfig settings = new NetworkConfig();

            //------WLAN --
            settings.SSID = response["ESSID"];
            WifiMode mode;
            if (response["Mode"] == "Managed")
                mode = WifiMode.WirelessNetwork;
            else
                mode = WifiMode.Computer2ComputerNetwork;

            settings.Key = response["Key"];
            settings.Channel = int.Parse(response["Channel"]);

            //WepSet
            //WepAsc
            //WepGroup
            //Wep64type
            //Wep128type
            //CurrentWiFiState = OK


            //------ IP --
            //CameraName


            settings.Mode = mode;
            //dhcp / manually
            settings.DHCPEnabled = (response["IPWay"] == "dhcp");
            settings.RovioIPAddress = response["IP"];
            settings.SubnetMask = response["Netmask"];
            settings.DefaultGateway = response["Gateway"];
            settings.DNS = response["DNS0"];
            //DNS1
            //DNS2
            //Enable = 1


            settings.CurrentIP = response["CurrentIP"];
            settings.CurrentNetmask = response["CurrentNetmask"];
            settings.CurrentGateway = response["CurrentGateway"];
            settings.CurrentDNS0 = response["CurrentDNS0"];
            //CurrentDNS1
            //CurrentDNS2
            //CurrentIPState = STATIC_IP_OK



            //------ MAC --
            settings.MACAddress = response["MAC"];


            //------ HTTP --
            settings.WebPort = int.Parse(response["Port0"]);
            //Port1

            return settings;
        }      
    }
}
