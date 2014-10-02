using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    public class RovioLocation
    {
        /// <summary>
        /// [-32767, 32768]
        /// </summary>
        public Int16 X { get; private set; }
        /// <summary>
        /// [-32767, 32768]
        /// </summary>
        public Int16 Y { get; private set; }
        /// <summary>
        /// [-PI, PI] 弧度
        /// </summary>
        public double Theta { get; private set; }
        /// <summary>
        /// [-180, 180] 角度
        /// </summary>
        public double Angle
        {
            get
            {
                return this.Theta * 180 / Math.PI;
            }
        }

        public RovioLocation(short x, short y, double theta)
        {
            this.X = x;
            this.Y = y;
            this.Theta = theta;
        }
    }
}
