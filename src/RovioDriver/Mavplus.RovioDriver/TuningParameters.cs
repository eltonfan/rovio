using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    /// <summary>
    /// 运动参数。
    /// </summary>
    public class TuningParameters
    {
        /// <summary>
        /// 左右移动的速度。
        /// </summary>
        public byte LeftRight { get; set; }
        /// <summary>
        /// 前进的速度。
        /// </summary>
        public byte Forward { get; set; }
        /// <summary>
        /// 后退的速度。
        /// </summary>
        public byte Reverse { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte DriveTurn { get; set; }
        public byte HomingTurn { get; set; }
        public byte ManDrive { get; set; }
        public byte ManTurn { get; set; }
        public int DockTimeout { get; set; }
    }
}
