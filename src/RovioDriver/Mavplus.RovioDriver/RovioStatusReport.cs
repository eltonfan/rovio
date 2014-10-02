using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Mavplus.RovioDriver.API;
using System.ComponentModel;

namespace Mavplus.RovioDriver
{
    /// <summary>
    /// Status of the Rovio
    /// </summary>
    public enum RovioStates
    {
        Idle = 0,
        DrivingHome = 1,
        Docking = 2,
        ExecutingPath = 3,
        RecordingPath = 4,
        SavingHome = 5,
    }

    [Flags]
    public enum RovioFlags : ushort
    {
        None = 0,
        HomePosition = 1,
        ObstacleDetected = 2,
        IRDetectorActivated = 4,
    }
    /// <summary>
    /// Video compression
    /// </summary>
    public enum VideoCompression
    {
        [Description("低")]
        Low = 0,
        [Description("中")]
        Med = 1,
        [Description("高")]
        High = 2,
    }

    /// <summary>
    /// user privilege status.
    /// </summary>
    public enum UserPrivilege
    {
        /// <summary>
        /// administrator
        /// </summary>
        Administrator = 0,
        /// <summary>
        /// guest user.
        /// </summary>
        Guest = 1,
    }
    /// <summary>
    /// 交流电频率。
    /// </summary>
    public enum ACFrequency
    {
        /// <summary>
        /// NotDetected
        /// </summary>
        AutoDetect = 0,
        _50Hz = 1,
        _60Hz = 2,
    }

    public enum BatteryStates
    {
        Normal,
        /// <summary>
        /// docked but not charging
        /// </summary>
        NotCharging,
        /// <summary>
        /// completed
        /// </summary>
        Completed,
        /// <summary>
        /// charging
        /// </summary>
        charging,
    }

    public enum HeadPositions
    {
        Unknown = 0,
        Down = 1,
        Middle = 2,
        Up = 3,
    }

    public class RovioStatusReport
    {
        string str = "";

        public override string ToString()
        {
            return this.str;
        }

        /// <summary>
        /// 相对信号最强的信标，Rovio的平均位置。
        /// 
        /// Average location of Rovio in relation to the strongest room beacon
        /// </summary>
        public RovioLocation Location { get; private set; }
        /// <summary>
        /// Room ID. -1标识没有发现房间，或者房间信号太弱。
        /// 
        /// 0 = Home base.
        /// 1-9 = Mutable room projector.
        /// </summary>
        public int RoomID { get; private set; }
        /// <summary>
        /// Navigation Signal strength.
        /// 
        /// 0 – 65535 (16bit)
        /// (Strong signal &gt; 47000)
        /// (No signal &lt; 5000)
        /// </summary>
        public double NavigationSignalStrength { get; private set; }
        /// <summary>
        /// Signal strength for docking beacon when available.
        /// 
        /// 0 - 65535 (16bit)
        /// </summary>
        public UInt16 BeaconSignalStrength { get; private set; }
        /// <summary>
        /// Horizontal poition of beacon as seen by NS
        /// 
        /// -32767 - 32768
        /// </summary>
        public Int16 BeaconPoitionX { get; private set; }
        /// <summary>
        /// The next strongest room beacon ID seen.
        /// 
        /// -1 = no room found.
        /// 1-9 = Mutable room ID
        /// </summary>
        public int NextRoomID { get; private set; }
        /// <summary>
        /// The signal strength of the next strongest room beacon.
        /// 
        /// 0 – 65535 (16bit)
        /// (Strong signal &gt; 47000)
        /// (No signal &lt; 5000)
        /// </summary>
        public UInt16 NextRoomSS { get; private set; }
        /// <summary>
        /// Status of the Rovio
        /// </summary>
        public RovioStates State { get; private set; }
        public int ui_status { get; private set; }
        /// <summary>
        /// Status of robot resistance to drive into NS deprived areas
        /// 
        /// NOT IN USE
        /// </summary>
        public int resistance { get; private set; }
        /// <summary>
        /// Current status of the navigation state machine.
        /// 
        /// (For Debug purposes)
        /// </summary>
        public int sm { get; private set; }
        /// <summary>
        /// Current way point when using path.
        /// 
        /// 1 - 10
        /// </summary>
        public int WayPointIndex { get; private set; }
        /// <summary>
        /// Flags
        /// </summary>
        public RovioFlags Flags { get; private set; }

        /// <summary>
        /// Indicates the current brightness level
        /// 
        /// 1 (dimmest) – 6 (brightest)
        /// </summary>
        public byte brightness { get; private set; }

        public int ResolutionId { get; private set; }

        /// <summary>
        /// Video compression
        /// </summary>
        public VideoCompression VideoCompression { get; private set; }

        /// <summary>
        /// Frame rate.
        /// 
        /// 1 - 30
        /// </summary>
        public int frame_rate { get; set; }

        /// <summary>
        /// Show current user privilege status.
        /// </summary>
        public UserPrivilege Privilege { get; private set; }

        /// <summary>
        /// Whether need to have login and password.
        /// 
        /// 0 = request on username and password
        /// 1 = no request on username and password
        /// </summary>
        public bool AuthenticationRequired { get; private set; }
        /// <summary>
        /// Speaker Volume. [0.0, 1.0]
        /// </summary>
        public double speaker_volume { get; private set; }
        /// <summary>
        /// Microphone Volume. [0.0, 1.0]
        /// </summary>
        public double mic_volume { get; private set; }
        /// <summary>
        /// Wifi Signal strength. [0.0, 1.0]
        /// </summary>
        public double wifi_ss { get; private set; }

        /// <summary>
        /// Whether show time in the image.
        /// </summary>
        public bool show_time { get; private set; }

        /// <summary>
        /// DDNS update status
        /// </summary>
        public enum DdnsStates
        {
            NoUpdate = 0,
            Updating = 1,
            UpdateSuccessfully = 2,
            UpdateFailed = 3,
        }
        /// <summary>
        /// DDNS update status
        /// </summary>
        public DdnsStates ddns_state { get; private set; }

        /// <summary>
        /// Current status of e-mail client.
        /// 
        /// NOT IN USE
        /// </summary>
        public int email_state { get; private set; }
        /// <summary>
        /// Battery status
        /// </summary>
        public double BatteryLevel { get; private set; }
        /// <summary>
        /// Whether it is charging
        /// </summary>
        public BatteryStates charging { get; private set; }
        /// <summary>
        /// Head position
        /// 
        /// 204 = position low
        /// 135-140 = position mid-way
        /// 65 = position high
        /// </summary>
        public int HeadPositionTickCount { get; private set; }


        public HeadPositions HeadPosition
        {
            get
            {
                if(HeadPositionTickCount > 195 && HeadPositionTickCount < 205)
                    return HeadPositions.Down;
                if(HeadPositionTickCount > 130 && HeadPositionTickCount < 140)
                    return HeadPositions.Middle;
                if(HeadPositionTickCount > 60 && HeadPositionTickCount < 70)
                    return HeadPositions.Up;

                return HeadPositions.Unknown;
            }
        }

        /// <summary>
        /// Projector’s frequency
        /// </summary>
        public ACFrequency ACFrequency { get; private set; }

        internal static RovioStatusReport Parse(RovioStatusReport report, RovioResponse dic)
        {
            report.str = dic.ToString();

            report.Location = new RovioLocation(
                short.Parse(dic["x"]),
                short.Parse(dic["y"]),
                double.Parse(dic["theta"]));

            report.parseRoomId(dic["room"]);
            report.parseNavStrength(dic["ss"]);
            //如果信号太弱，将RoomId置为-1。
            if (!report.HasNavSignal)
                report.RoomID = -1;

            report.BeaconSignalStrength = UInt16.Parse(dic["beacon"]);
            report.BeaconPoitionX = Int16.Parse(dic["beacon_x"]);
            report.NextRoomID = int.Parse(dic["next_room"]);
            report.NextRoomSS = UInt16.Parse(dic["next_room_ss"]);

            report.parseNavState(dic["state"]);
            report.ui_status = int.Parse(dic["ui_status"]);
            report.resistance = int.Parse(dic["resistance"]);
            report.sm = int.Parse(dic["sm"]);
            report.WayPointIndex = int.Parse(dic["pp"]);
            report.parseFlags(dic["flags"]);

            report.brightness = byte.Parse(dic["brightness"]);

            report.ResolutionId = int.Parse(dic["resolution"]);
            report.VideoCompression = (VideoCompression)int.Parse(dic["video_compression"]);
            report.frame_rate = int.Parse(dic["frame_rate"]);
            report.Privilege = (UserPrivilege)int.Parse(dic["privilege"]);
            report.AuthenticationRequired = (int.Parse(dic["user_check"]) == 1);// ???? == 0

            //0 (lowest) – 31 (highest)
            report.speaker_volume = double.Parse(dic["speaker_volume"]) / 31.0;
            //0 (lowest) – 31 (highest)
            report.mic_volume = double.Parse(dic["mic_volume"]) / 31.0;
            report.parseWifiStrength(dic["wifi_ss"]);
            // 0 = Not showing the time. 1 = Showing time.
            report.show_time = (int.Parse(dic["show_time"]) == 1);
            report.ddns_state = (DdnsStates)int.Parse(dic["ddns_state"]);
            report.email_state = int.Parse(dic["email_state"]);
            report.parseBatteryStrength(dic["battery"]);
            report.parseCharging(dic["charging"]);
            report.HeadPositionTickCount = int.Parse(dic["head_position"]);
            report.ACFrequency = (ACFrequency)int.Parse(dic["ac_freq"]);

            return report;
        }

        public void Update(RovioResponse dic)
        {
            Parse(this, dic);
        }

        void parseFlags(string str)
        {
            this.Flags = (RovioFlags)UInt16.Parse(str, System.Globalization.NumberStyles.HexNumber);
            switch(this.Flags)
            {
                case RovioFlags.IRDetectorActivated:
                    break;
            }
        }

        void parseRoomId(string str)
        {
            this.RoomID = int.Parse(str);
        }

        void parseNavStrength(string str)
        {
            int value = int.Parse(str);
            if (value > 13000)
                this.NavigationSignalStrength = 1.00;
            else if (value > 10000)
                this.NavigationSignalStrength = 0.80;
            else if (value > 7000)
                this.NavigationSignalStrength = 0.60;
            else if (value > 4000)
                this.NavigationSignalStrength = 0.40;
            else if (value > 2000)
                this.NavigationSignalStrength = 0.20;
            else
                this.NavigationSignalStrength = 0.00;
        }
        
        int last_wifi = 255;
        void parseWifiStrength(string str)
        {
            //0 - 254
            if(this.GoingHome || this.PlayingPath)
                return;

            int value = int.Parse(str);
            var avg_value = (value + last_wifi) / 2;
            last_wifi = avg_value;
            if(avg_value > 201){ // 210
                this.wifi_ss = 1.00;
            } else if(avg_value > 191){ // 200
                this.wifi_ss = 0.80;
            } else if(avg_value > 181){ // 195
                this.wifi_ss = 0.60;
            } else if(avg_value > 171){ // 190
                this.wifi_ss = 0.40;
            } else if(avg_value > 166){ // 185
                this.wifi_ss = 0.20;
            }
            else
                this.wifi_ss = 0.00;// HACK: Remove when wifi strength is working
    
        }


        /// <summary>
        /// 是否有导航信号。
        /// </summary>
        public bool HasNavSignal
        {
            get { return (this.NavigationSignalStrength > 0.20); }
        }

        public bool SavingHome
        {
            get { return (this.State == RovioStates.SavingHome); }
        }
        public bool Recording
        {
            get { return (this.State == RovioStates.RecordingPath); }
        }
        public bool PlayingPath
        {
            get { return (this.State == RovioStates.ExecutingPath); }
        }
        public bool GoingHome
        {
            get { return (this.State == RovioStates.Docking); }
        }

        void parseNavState(string str)
        {
            this.State = (RovioStates)int.Parse(str);
        }

        static int[] battery_values = null;
        void parseBatteryStrength(string str){
            // <100 = turn itself off
            // 100-106 = try to go back home
            // 106-127 = normal
            int value = int.Parse(str);
            if (value <= 1)
                return;
            //if (is_moving)
            //    return;
            if (this.GoingHome || this.PlayingPath || this.Recording || this.SavingHome)
                return;

            if (battery_values == null)
            {//第一个数值
                battery_values = new int[5];
                for (int i = 0; i < battery_values.Length; i++)
                    battery_values[i] = value;
            }
            var avg_value = 0;
            for(int i = 0; i < 4; i++)
            {
                battery_values[i] = battery_values[i+1];
                avg_value += battery_values[i+1];
            }
            battery_values[4] = value;
            avg_value += value;
            avg_value = avg_value/5;
        
            if(avg_value > 122)
                this.BatteryLevel = 1.00;
            else if(avg_value > 117)
                this.BatteryLevel = 0.80;
            else if(avg_value > 115)
                this.BatteryLevel = 0.60;
            else if(avg_value > 112)
                this.BatteryLevel = 0.40;
            else if(avg_value > 108)
                this.BatteryLevel = 0.20;
            else
                this.BatteryLevel = 0.00;
        }
        void parseCharging(string str)
        {
            // 0-79 = not charging,  80 = charging
            int value = int.Parse(str);
            if (value == 0)
                this.charging = BatteryStates.Normal;
            else if (value == 64)
                this.charging = BatteryStates.NotCharging;
            else if (value == 72)
                this.charging = BatteryStates.Completed;
            else if (value == 80)
                this.charging = BatteryStates.charging;
            else
            { }
        }
    }
}
