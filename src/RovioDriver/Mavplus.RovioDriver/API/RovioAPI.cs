using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.ComponentModel;

namespace Mavplus.RovioDriver.API
{
    /// <summary>
    /// Rovio机器人功能封装，用于访问机器人API。
    /// Class for accessing Rovio API。
    /// </summary>
    internal partial class RovioAPI
    {
        readonly RovioWebClient rwc = null;
        readonly MovementController movement = null;
        readonly ManualDriver manualDriver = null;
        readonly CameraController camera = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="RovioAPI" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public RovioAPI(string host, int port, NetworkCredential credentials)
        {
            this.rwc = new RovioWebClient(host, port, credentials);

            this.movement = new MovementController(this);
            this.manualDriver = this.movement.ManualDriver;
            this.camera = new CameraController(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RovioAPI" /> class.
        /// </summary>
        /// <param name="address">The address to acces Rovio.</param>
        /// <param name="username">The username to acces Rovio.</param>
        /// <param name="password">The password to acces Rovio.</param>
        public RovioAPI(string host, int port, string username, string password)
            : this(host, port,  new NetworkCredential(username, password))
        { }

        internal RovioResponse Request(string url, params RequestItem[] parameters)
        {
            return rwc.Request(url, this.CommandTimeout, parameters);
        }
        internal RovioResponse Request(string url, int timeout, params RequestItem[] parameters)
        {
            return rwc.Request(url, timeout, parameters);
        }

        internal byte[] DownloadData(string cmd)
        {
            return rwc.DownloadData(cmd);
        }

        /// <summary>
        /// 等待命令执行的时间（以毫秒为单位）。 默认值为 10*1000 毫秒。
        /// </summary>
        public int CommandTimeout { get; set; }

        public MovementController Movement
        {
            get { return this.movement; }
        }
        /// <summary>
        /// Accepts manual driving commands.
        /// </summary>
        public ManualDriver ManualDriver
        {
            get { return this.movement.ManualDriver; }
        }
        public CameraController Camera
        {
            get { return this.camera; }
        }
    }
}
