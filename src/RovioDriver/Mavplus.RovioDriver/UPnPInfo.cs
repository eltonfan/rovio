using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver
{
    public class UPnPInfo
    {
        public bool Enable { get; private set; }
        public int Port { get; private set; }
        public string IP;
        public int HttpPort;
        public int RstpTcpPort;
        public int RstpUdpPort;

        public static UPnPInfo Parse(RovioResponse response)
        {
            UPnPInfo info = new UPnPInfo();
            info.Enable = (response["Enable"] == "1");
            info.Port = int.Parse(response["Port"]);
            info.IP = response["IP"];
            info.HttpPort = int.Parse(response["HTTP"]);
            info.RstpTcpPort = int.Parse(response["RTSP_TCP"]);
            info.RstpUdpPort = int.Parse(response["RTSP_UDP"]);

            return info;
        }
    }
}
