using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Amib.Threading;
using Mavplus.RovioDriver.API;
using System.ComponentModel;

namespace Mavplus.RovioDriver
{
    public partial class Rovio
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Rovio状态刷新的时间间隔。
        /// </summary>
        public const int STATUS_INTERVAL = 200;
        /// <summary>
        /// HTTP请求的最大数量。
        /// </summary>
        public const int MAX_REQUESTS = 10;

        public const int DEFAULT_SPEED = 5;
        public const int DEFAULT_TURN_SPEED = 5;
        public const int DEFAULT_ROT_SPEED = 2;

        RovioAPI api = null;
        readonly SmartThreadPool threadPool = null;
        /// <summary>
        /// 提供状态更新。
        /// </summary>
        readonly System.Timers.Timer timerRefreshStatus = null;
        readonly List<string> listWays = null;
        readonly RovioSettings settings = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="Rovio" /> class.
        /// </summary>
        public Rovio()
        {
            log.Info("Rovio 初始化 ...");
            this.threadPool = new SmartThreadPool(1000, MAX_REQUESTS);

            this.timerRefreshStatus = new System.Timers.Timer();
            this.timerRefreshStatus.Interval = STATUS_INTERVAL;
            this.timerRefreshStatus.AutoReset = false;
            this.timerRefreshStatus.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e)
            {
                threadPool.QueueWorkItem(UpdateStatus);
                //UpdateStatus();
            };

            this.listWays = new List<string>();
            this.settings = new RovioSettings(this);

            InitMovement();
            log.Info("Rovio 初始化完毕。");
        }

        internal RovioAPI API { get { return this.api; } }

        [Obsolete("未实现。")]
        public string DownloadString(string relativeUri)
        {
            return "";
        }
        internal RovioResponse Request(string url, params RequestItem[] parameters)
        {
            if (api == null)
                throw new Exception("Rovio 尚未连接。");
            return api.Request(url, parameters);
        }

        internal byte[] DownloadData(string cmd)
        {
            if (api == null)
                throw new Exception("Rovio 尚未连接。");
            return api.DownloadData(cmd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">The address to acces Rovio.</param>
        /// <param name="username">The username to acces Rovio.</param>
        /// <param name="password">The password to acces Rovio.</param>
        public void Open(string host, int port, int rtspPort, string username, string password, BackgroundWorker bw = null)
        {
            this.Open(host, port, rtspPort, new NetworkCredential(username, password), bw);
        }

        public void Open(string host, int port, int rtspPort, NetworkCredential credentials, BackgroundWorker bw = null)
        {
            try
            {
                this.api = new RovioAPI(host, port, credentials);
                this.api.CommandTimeout = this.commandTimeout;
                this.RtspUrl = string.Format("rtsp://{0}:{1}@{2}{3}/webcam",
                    credentials.UserName,
                    credentials.Password,
                    host,
                    (rtspPort == 554) ? "" : ":" + rtspPort);
                this.MJpegUrl = string.Format("http://{0}{1}/GetData.cgi",
                    host, ((port == 80) ? "" : ":" + port));
                
                if (bw != null)
                    bw.ReportProgress(0, string.Format("登录到 {0}{1} ...",
                        host, (port == 80) ? "" : ":" + port));

                //检查是否登录
                this.userInfo = api.GetMyself(true);

                FirmwareVersion firmware = api.GetVer();
                string libNSVersion = api.Movement.GetLibNSVersion();
                this.Version = new RovioVersionInfo(firmware, libNSVersion);


                if (bw != null)
                    bw.ReportProgress(0, "登录成功，载入Rovio配置...");
                this.settings.Load();


                this.networkConfig = api.GetNetworkConfig();

                //resetMovementSettings
                this.movement_speed = DEFAULT_SPEED;
                this.turn_speed = DEFAULT_TURN_SPEED;
                this.rot_speed = DEFAULT_ROT_SPEED;

                // upnp settings need to be called as soon as possible for RTSP feeds
                //loadUPnPFields
                this.upnp_info = api.GetUPnP();
                //updateOnlineStatus();
                //updateUPnPFields();
                //initial_upnp_load = false;

                // need to know web port for ActiveX
                //loadWebPort
                this.net_web_port = api.GetHttp();

                // need to know the manual external ip

                //refreshPathList
                this.listWays.Clear();
                string[] list = api.Movement.GetPathList();
                this.listWays.AddRange(list);


                //loadForceMJPEGFromURL();

                //var force_reboot = getQueryVariable("reboot");
                //if(force_reboot != null && parseInt(force_reboot)){
                //    $('settings_dialog').style.display = 'none';
                //    selectTab($('sidetab_1'));
                //    initReboot();
                //    return;
                //}


                //access_settings_panel = 1;
                //$('move_ir').checked = IR_val;

                if (userInfo.Group == UserGroups.Administrator)
                {//管理员账号
                    // need to know if we are using a domain for checking external access
                    //loadDynDNSSettings();

                    ////loadSettingsPanels();
                    //setServerTime();
                    //getEvoVersion();
                    //getWBVersion();
                    //loadSMTPSettings();

                    //setTimeout('getLatestVersion()',5000);
                }

                //if(!user_guest){
                //setTimeout("access_settings_panel = 1",1000);
                //}

                RovioStatusReport report = api.Movement.GetReport();
                this.status = report;
                timerRefreshStatus.Start();
                timerMovement.Start();

                StartWorkerThread();
                isOpen = true;
            }
            catch (Exception ex)
            {
                this.api = null;
                isOpen = false;

                throw ex;
            }
        }

        public bool CheckConnection()
        {
            try
            {
                RovioResponse response = api.Request("/GetMyself.cgi", 2000,
                new RequestItem("ShowPrivilege", 0));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Close()
        {
            if(!isOpen)
                throw new InvalidOperationException("The client is not yet connected");
            try
            {
                this.settings.Save();
            }
            catch (Exception ex)
            {

            }

            StopWorkerThread();
            timerRefreshStatus.Stop();
            timerMovement.Stop();
            isOpen = false;
        }

        public event EventHandler<StatusChangedEventArgs> StatusChanged;
        void UpdateStatus()
        {
            try
            {
                BatteryStates oldBatteryStates = BatteryStates.NotCharging;
                if (this.status != null)
                    oldBatteryStates = this.status.charging;

                RovioStatusReport report = api.Movement.GetReport();
                if (report == null)
                    return;

                if(oldBatteryStates != BatteryStates.Normal && report.charging == BatteryStates.Normal)
                {//如果开始在充电位置，现在活动
                    this.settings.SetBlueLight(this.settings.BlueLights, true);
                }
                //!!!注意，这里的status引用有可能本来就是report。因为report实例不重新初始化。
                this.status = report;

                if(this.StatusChanged != null)
                    this.StatusChanged(this, new StatusChangedEventArgs(report));
            }
            catch (Exception)
            { }
            finally
            {
                timerRefreshStatus.Start();
            }
        }

        public WifiInfo[] ScanWlan()
        {
            if (api == null)
                throw new Exception("Rovio 尚未连接。");
            return api.ScanWlan();
        }
    }
}
