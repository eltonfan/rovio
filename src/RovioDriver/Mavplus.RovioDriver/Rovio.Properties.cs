using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Mavplus.RovioDriver
{
    partial class Rovio
    {
        int commandTimeout = 10 * 1000;
        /// <summary>
        /// 等待命令执行的时间（以毫秒为单位）。 默认值为 10*1000 毫秒。
        /// </summary>
        public int CommandTimeout
        {
            get { return this.commandTimeout; }
            set
            {
                if (this.commandTimeout == value)
                    return;
                this.commandTimeout = value;
                if (this.api != null)
                    this.api.CommandTimeout = this.commandTimeout;
            }
        }
        /// <summary>
        /// 固件信息。
        /// </summary>
        public RovioVersionInfo Version { get; private set; }

        public RovioSettings Settings
        {
            get { return this.settings; }
        }

        /// <summary>
        /// RTSP视频的访问地址。
        /// </summary>
        public string RtspUrl { get; private set; }

        public string MJpegUrl { get; private set; }

        bool isOpen = false;
        public bool IsOpen
        {
            get { return this.isOpen; }
        }

        RovioStatusReport status = null;
        public RovioStatusReport CurrentStatus
        {
            get { return this.status; }
        }

        /// <summary>
        /// 当前登录信息。
        /// </summary>
        UserInformation userInfo = null;
        public UserInformation UserInfo
        {
            get { return this.userInfo; }
        }
        NetworkConfig networkConfig = null;
        NetworkConfig NetworkConfig
        {
            get { return this.networkConfig; }
        }

        int movement_speed = DEFAULT_SPEED;
        int turn_speed = DEFAULT_TURN_SPEED;
        int rot_speed = DEFAULT_ROT_SPEED;



        public double MovementSpeed
        {
            get { return this.movement_speed; }
        }

        /// <summary>
        /// 状态刷新的频率，毫秒。默认是200毫秒。
        /// </summary>
        [DefaultValue(200)]
        public int StatusRefreshInterval
        {
            get { return (int)timerRefreshStatus.Interval; }
            set { timerRefreshStatus.Interval = value; }
        }

        int net_web_port = 0;

        UPnPInfo upnp_info = null;
    }
}
