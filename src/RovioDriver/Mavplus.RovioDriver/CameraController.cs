using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver
{
    /// <summary>
    /// 相机控制。
    /// </summary>
    internal class CameraController
    {
        readonly RovioAPI rovio = null;
        public CameraController(RovioAPI rovio)
        {
            this.rovio = rovio;
        }

        /// <summary>
        /// The basic command for acquiring MJPEG.
        /// <remarks>NOT IMPLEMENTED</remarks>
        /// </summary>
        /// <returns></returns>
        public string GetData()
        {
            RovioResponse response = rovio.Request("GetData.cgi");
            return "";
        }

        /// <summary>
        /// The basic command for acquiring Image.
        /// </summary>
        /// <returns>Bitmap</returns>
        public Bitmap GetImage()
        {
            byte[] buf = rovio.DownloadData("Jpeg/CamImg0000.jpg");
            return Image.FromStream(new MemoryStream(buf)) as Bitmap;
        }

        /// <summary>
        /// Change the resolution setting of camera's images.
        /// </summary>
        /// <param name="ResType">Camera supports 4 types of resolution:0 - {176, 144}1 - {352, 288}2 - {320, 240} (Default)3 - {640, 480}</param>
        /// <returns></returns>
        public void ChangeResolution(int ResType)
        {
            RovioResponse response = rovio.Request("ChangeResolution.cgi?ResType=" + ResType.ToString());
        }

        /// <summary>
        /// Change the quality setting of camera's images. (only available with MPEG4)
        /// </summary>
        /// <param name="ratio">0 – 2 (representing low, medium and high quality respectively)</param>
        /// <returns></returns>
        public void ChangeCompressRatio(VideoCompression ratio)
        {
            RovioResponse response = rovio.Request("ChangeCompressRatio.cgi?Ratio=" + ratio.ToString());
        }


        /// <summary>
        /// Change the frame rate setting of camera's images.
        /// </summary>
        /// <param name="framerate">2 – 32 frame per seconds respectively</param>
        /// <returns></returns>
        public void ChangeFramerate(int framerate)
        {
            RovioResponse response = rovio.Request("ChangeFramerate.cgi?Framerate=" + framerate.ToString());
        }


        /// <summary>
        /// Change the brightness setting of camera's images.
        /// </summary>
        /// <param name="brightness">0 - 6 (The lower the value is, the dimmer the image is)</param>
        /// <returns></returns>
        public void ChangeBrightness(int brightness)
        {
            RovioResponse response = rovio.Request("ChangeBrightness.cgi?Brightness=" + brightness.ToString());
        }

        /// <summary>
        /// Change the Speaker Volume setting of camera.
        /// </summary>
        /// <param name="SpeakerVolume">[0.0, 1.0]</param>
        /// <returns></returns>
        public void ChangeSpeakerVolume(double volume)
        {
            //Speaker Volume : 0 - 31 (The lower the value is, the lower the speaker volume is)
            int rawValue = RovioAPI.SetDouble(volume, 0, 31);
            RovioResponse response = rovio.Request("ChangeSpeakerVolume.cgi?SpeakerVolume=" + rawValue);
        }

        /// <summary>
        /// Change the Mic Volume setting of IP_Cam.
        /// </summary>
        /// <param name="MicVolume">[0.0, 1.0]</param>
        /// <returns></returns>
        public void ChangeMicVolume(double volume)
        {
            //Mic Volume : 0 - 31 (The lower the value is, the lower the mic volume is)
            int rawValue = RovioAPI.SetDouble(volume, 0, 31);
            RovioResponse response = rovio.Request("ChangeMicVolume.cgi?MicVolume=" + rawValue);
        }

        /// <summary>
        /// Change camera sensor’s settings.
        /// </summary>
        /// <param name="Frequency">50 – 50Hz, 60 – 60Hz, 0 – Auto detect</param>
        /// <returns></returns>
        public string SetCamera(ACFrequency frequency)
        {
            int number = 50;
            switch (frequency)
            {
                case ACFrequency._50Hz:
                    number = 50;
                    break;
                case ACFrequency._60Hz:
                    number = 60;
                    break;
                case ACFrequency.AutoDetect:
                    number = 0;
                    break;
            }
            RovioResponse response = rovio.Request("SetCamera.cgi?Frequency=" + number.ToString());
            return "";
        }

        /// <summary>
        /// Get the camera sensor’s settings.
        /// </summary>
        /// <returns></returns>
        public string GetCamera()
        {
            RovioResponse response = rovio.Request("GetCamera.cgi");
            return "";
        }
    }
}
