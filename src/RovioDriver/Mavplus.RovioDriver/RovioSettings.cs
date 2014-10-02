using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mavplus.RovioDriver.API;
using System.Drawing;

namespace Mavplus.RovioDriver
{
    /// <summary>
    /// 视频播放器。默认值是2 MJPEG
    /// </summary>
    public enum VideoPlayers
    {
        ActiveX = 0,
        Quicktime = 1,
        MJPEG = 2,
        /// <summary>
        /// VLC + JavaApplet (option currently unstable especially on mac)
        /// </summary>
        VLC = 3,
    }

    public abstract class SettingGroup
    {
        protected bool modified = false;
        /// <summary>
        /// 获取或设置一个值，该值指示自创建文本框控件或上次设置该控件的内容后，用户修改了该控件。
        /// </summary>
        /// <value>如果控件的内容被修改了，则为 true，否则为 false。 默认值为 false。 </value>
        public bool Modified
        {
            get { return this.modified; }
        }

        internal abstract void Load(Dictionary<FlashParameters, Int32> flashParameters, RovioStatusReport report, RovioMcuReport mcuReport);

        public abstract void Save();
    }

    /// <summary>
    /// Rovio设置。
    /// </summary>
    public partial class RovioSettings
    {

        const int DEFAULT_OTHER_PLAYER = 2;
        /// <summary>
        /// Rovio的名称。
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Rovio主控板时间。
        /// </summary>
        public DateTime DateTime { get; set; }




        /// <summary>
        /// 保持图像的原始比例。
        /// </summary>
        public bool MaintainAspectRatio { get; set; }


        /// <summary>
        /// 视频播放器。默认值是2 MJPEG
        /// i_SVP   safari video player
        /// </summary>
        public VideoPlayers PlayerType { get; private set; }
        /// <summary>
        /// 固件更新提示。Alert me when new firmware is available
        /// </summary>
        public bool FirmwareAlert { get; private set; }

        /// <summary>
        /// 障碍检测开启。
        /// </summary>
        public bool IrDetectEnabled { get; set; }

        public HeadLightState HeadLight { get; private set; }
        public BlueLightState BlueLights { get; private set; }
        public NightMode NightMode { get; private set; }

        /// <summary>
        /// Speaker Volume. [0.0, 1.0]
        /// </summary>
        public double SpeakerVolume { get; private set; }
        /// <summary>
        /// Microphone Volume. [0.0, 1.0]
        /// </summary>
        public double MicrophoneVolume { get; private set; }


        readonly VideoGroup groupVideo = null;
        readonly MovementGroup groupMovement = null;
        readonly MailGroup groupMail = null;
        readonly Rovio rovio = null;
        internal RovioSettings(Rovio rovio)
        {
            this.rovio = rovio;

            this.groupVideo = new VideoGroup(this);
            this.groupMovement = new MovementGroup(this);
            this.groupMail = new MailGroup(this);
        }

        public VideoGroup Video
        {
            get { return this.groupVideo; }
        }
        public MovementGroup Movement
        {
            get { return this.groupMovement; }
        }
        public MailGroup Mail
        {
            get { return this.groupMail; }
        }

        public void Load()
        {
            RovioAPI api = rovio.API;
            MovementController movement = api.Movement;
            CameraController camera = api.Camera;
            RovioStatusReport report = movement.GetReport();
            RovioMcuReport mcuReport = movement.GetMCUReport();
            Dictionary<FlashParameters, Int32> flashParameters = movement.ReadAllParameters();


            this.Name = api.GetName();
            this.DateTime = api.GetTime();
            
            this.MaintainAspectRatio = RovioAPI.GetBoolean(flashParameters[FlashParameters.MaintainAspectRatio]);

            ///// <summary>
            ///// i_LR
            ///// </summary>  
            //latency = 4,

            this.PlayerType = (VideoPlayers)flashParameters[FlashParameters.video_player];
            ///// <summary>
            ///// i_UPnP  upnp just enabled
            ///// </summary>
            //upnp_just_enabled = 6,
            ///// <summary>
            ///// i_SS    show status
            ///// </summary>
            //show_online_status = 7,
            ///// <summary>
            ///// i_MIIP1  manual ip
            ///// </summary>
            //manual_internetip = 8,
            ///// <summary>
            ///// i_MIIP2   manual ip
            ///// </summary>
            //manual_internetip2 = 9,
            ///// <summary>
            ///// i_VIA    verify internet access
            ///// </summary>
            //net_verify_access = 10,
            this.FirmwareAlert = RovioAPI.GetBoolean(
                flashParameters[FlashParameters.firmware_alert]);
            ///// <summary>
            ///// i_AVF    auto set video frequency
            ///// </summary>
            //video_freq = 12,
            
            this.IrDetectEnabled = ((report.Flags & RovioFlags.IRDetectorActivated) == RovioFlags.IRDetectorActivated);

            this.HeadLight = mcuReport.HeadLight;
            this.BlueLights = (BlueLightState)RovioAPI.GetByte(flashParameters[FlashParameters.BlueLights], (byte)BlueLightState.All);
            this.NightMode = (NightMode)RovioAPI.GetByte(flashParameters[FlashParameters.NightMode], (byte)NightMode.Normal);

            this.SpeakerVolume = report.speaker_volume;
            this.MicrophoneVolume = report.mic_volume;

            groupVideo.Load(flashParameters, report, mcuReport);
            groupMovement.Load(flashParameters, report, mcuReport);
            groupMail.Load(flashParameters, report, mcuReport);
        }

        public void Save()
        {
            groupVideo.Save();
            groupMovement.Save();
            groupMail.Save();
        }

        public void SaveName(string value)
        {
            this.Name = value;
            rovio.API.SetName(value);
            this.Name = rovio.API.GetName();
        }

        public void SavePlayer(VideoPlayers value)
        {
            this.PlayerType = value;

            RovioAPI api = rovio.API;
            MovementController movement = api.Movement;
            movement.SaveParameter(FlashParameters.video_player, (int)this.PlayerType);
        }

        public void SetFirmwareAlert(bool value)
        {
            if(this.FirmwareAlert == value)
                return;
            this.FirmwareAlert = value;

            RovioAPI api = rovio.API;
            MovementController movement = api.Movement;
            movement.SaveParameter(
                new FlashParameterItem(FlashParameters.firmware_alert, RovioAPI.SetBoolean(value)));
        }


        public void SetIRState(bool irDetectEnabled)
        {
            this.IrDetectEnabled = irDetectEnabled;

            RovioAPI api = rovio.API;
            MovementController movement = api.Movement;
            movement.SetIRState(this.IrDetectEnabled);
        }

        public void SetHeadLight(HeadLightState state)
        {
            this.HeadLight = state;

            RovioAPI api = rovio.API;
            MovementController movement = api.Movement;
            movement.SetHeadLight(this.HeadLight);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="force">如果强制刷新，则无论值有无改变都重新写入到设备。</param>
        public void SetBlueLight(BlueLightState value, bool force = false)
        {
            if (this.BlueLights == value && !force)
                return;
            this.BlueLights = value;

            RovioAPI api = rovio.API;
            MovementController movement = api.Movement;
            api.SetLED(this.BlueLights);

            if (this.BlueLightStateChanged != null)
                this.BlueLightStateChanged(this, EventArgs.Empty);
        }
        public event EventHandler BlueLightStateChanged;

        public void SetNightMode(NightMode mode)
        {
            this.NightMode = mode;

            RovioAPI api = rovio.API;
            MovementController movement = api.Movement;
            api.SetNight(this.NightMode);
        }

        public void SaveSpeakerVolume(double volume)
        {
            this.SpeakerVolume = volume;

            RovioAPI api = rovio.API;
            CameraController camera = api.Camera;
            camera.ChangeSpeakerVolume(volume);
        }

        public void SaveMicrophoneVolume(double volume)
        {
            this.MicrophoneVolume = volume;

            RovioAPI api = rovio.API;
            CameraController camera = api.Camera;
            camera.ChangeMicVolume(volume);
        }
    }
}
