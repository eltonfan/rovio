using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Mavplus.RovioDriver.API
{
    partial class RovioAPI
    {

        /// <summary>
        /// Set a logo string on the image.
        /// </summary>
        /// <param name="showstring">time - time, date - date,ver - version</param>
        /// <param name="pos">0 – top left, 1 – top right, 2 – bottom left, 3 – bottom right</param>
        /// <returns></returns>
        public void SetLogo(string showstring, LogoPostions position)
        {
            RovioResponse response = this.Request("/SetLogo.cgi",
                new RequestItem("showstring", showstring),
                new RequestItem("pos", (int)position));
        }

        /// <summary>
        /// Get a logo string on the image.
        /// </summary>
        /// <returns></returns>
        public string GetLogo()
        {
            RovioResponse response = this.Request("/GetLogo.cgi");
            return "";
        }

        /// <summary>
        /// Get IP settings.
        /// </summary>
        /// <param name="Interface">eth1, wlan0</param>
        /// <returns></returns>
        public string GetIP(string Interface)
        {
            RovioResponse response = this.Request("/GetIP.cgi",
                new RequestItem("Interface", Interface));
            return "";
        }

        /// <summary>
        /// Get WiFi settings.
        /// </summary>
        /// <returns></returns>
        public string GetWlan()
        {
            RovioResponse response = this.Request("/GetWlan.cgi");
            return "";
        }

        /// <summary>
        /// Get DDNS settings.
        /// </summary>
        /// <returns></returns>
        public string GetDDNS()
        {
            RovioResponse response = this.Request("/GetDDNS.cgi");
            return response["DNS0"];
        }

        /// <summary>
        /// Set Mac address.
        /// </summary>
        /// <param name="MAC">Mac address</param>
        /// <returns></returns>
        public bool SetMac(string MAC)
        {
            RovioResponse response = this.Request("/SetMac.cgi",
                new RequestItem("MAC", MAC));
            return true;
        }

        /// <summary>
        /// Get Mac address.
        /// </summary>
        /// <returns></returns>
        public string GetMac()
        {
            RovioResponse response = this.Request("GetMac.cgi");
            return response["MAC"];
        }

        /// <summary>
        /// Get HTTP server's settings.
        /// </summary>
        /// <returns></returns>
        public int GetHttp()
        {
            RovioResponse response = this.Request("/GetHttp.cgi");

            int WebPort = int.Parse(response["Port0"]);
            //Port1

            return WebPort;
        }

        /// <summary>
        /// Set camera's name.
        /// </summary>
        /// <param name="CameraName"></param>
        /// <returns></returns>
        public void SetName(string name)
        {
            RovioResponse response = this.Request("/SetName.cgi",
                new RequestItem("CameraName", name));
        }

        /// <summary>
        /// Get camera's name.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            RovioResponse response = this.Request("/GetName.cgi");
            if (response == null || !response.ContainsKey("CameraName"))
                return null;

            return response["CameraName"];
        }

        /// <summary>
        /// Get run-time status of Rovio.
        /// </summary>
        /// <returns></returns>
        public RovioStatus GetStatus()
        {
            RovioResponse response = this.Request("/GetStatus.cgi");
            return RovioStatus.Parse(response);
        }

        /// <summary>
        /// Get Rovio’s system logs information.
        /// </summary>
        /// <returns></returns>
        public string GetLog()
        {
            RovioResponse response = this.Request("/GetLog.cgi");
            return "";
        }

        /// <summary>
        /// Get Rovio’s base firmware version, Rovio also has a UI version and a NS2 version this function only get the base OS version.
        /// </summary>
        /// <returns></returns>
        public FirmwareVersion GetVer()
        {
            //Version = Jan 12 2010 14:41:24 $Revision: 5.3503$
            RovioResponse response = this.Request("/GetVer.cgi");
            return FirmwareVersion.Parse(response["Version"]);
        }

        /// <summary>
        /// Change all settings to factory-default.
        /// </summary>
        /// <returns></returns>
        public void SetFactoryDefault()
        {
            RovioResponse response = this.Request("/SetFactoryDefault.cgi");
        }

        /// <summary>
        /// Reboot Rovio.
        /// </summary>
        /// <returns></returns>
        public void Reboot()
        {
            RovioResponse response = this.Request("/Reboot.cgi");
        }

        /// <summary>
        /// Send audio to server and playback at server side
        /// Rovio 接受 PCM 8KHz 16bit Mono(单声道) 的内容。
        /// </summary>
        public void GetAudio(byte[] audio, int offset, int length, BackgroundWorker bw = null)
        {
            this.rwc.UploadData("/GetAudio.cgi", audio, offset, length, 30000, bw);
        }

        /// <summary>
        /// Set the media format.
        /// </summary>
        /// <param name="Audio">0 – 4</param>
        /// <param name="Video">0 – 1</param>
        /// <returns></returns>
        public string SetMediaFormat(int Audio, int Video)
        {
            RovioResponse response = this.Request("/SetMediaFormat.cgi",
               new RequestItem("Audio", Audio.ToString()),
               new RequestItem("Video", Video.ToString()));
            return "";
        }

        /// <summary>
        /// Get the media format.
        /// </summary>
        /// <returns></returns>
        public string GetMediaFormat()
        {
            RovioResponse response = this.Request("/GetMediaFormat.cgi");
            return "";
        }
        public UPnPInfo GetUPnP()
        {
            RovioResponse response = this.Request("/GetUPnP.cgi");

            return UPnPInfo.Parse(response);
        }
        public void SetLED(BlueLightState mode)
        {
            movement.SaveParameter(FlashParameters.BlueLights, (byte)mode);
            RovioResponse response = this.Request("/mcu",
                new RequestItem("parameters", string.Format("114D4D00010053485254000100011A{0:X2}0000", (byte)mode)));
        }
        public void SetNight(NightMode mode)
        {
            movement.SaveParameter(FlashParameters.NightMode, (byte)mode);

            RovioResponse response = this.Request("/debug.cgi",
                new RequestItem("action", "write_i2c"),
                new RequestItem("address", "0x14"),
                new RequestItem("value", "0x" + ((int)mode).ToString("X2")));
  		}
        /// <summary>
        /// 读取夜视设置。
        /// </summary>
        public void GetNight()
        {
            // roviosrc20100408.7z\Host\LibCamera\Src\LibIPCamera.c
            // int Config_MemDebug(HTTPCONNECTION hConnection, LIST *pParamList, int iAction, XML *pReturnXML)

            RovioResponse response = this.Request("/debug.cgi",
                new RequestItem("action", "read_i2c"),
                new RequestItem("address", "0x14"));
            //
        }
    }
}