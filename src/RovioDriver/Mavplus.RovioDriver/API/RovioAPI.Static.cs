using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver.API
{
    partial class RovioAPI
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static bool GetBoolean(int raw)
        {
            return (raw != 0);
        }
        public static int SetBoolean(bool value)
        {
            return value ? 1 : 0;
        }
        /// <summary>
        /// 得到 [0.0, 1.0] 值。如果小于0，则返回默认值。
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static double GetDouble(int raw, int min, int max, double defaultValue)
        {
            if (raw < 0)
                return defaultValue;
            double value = (double)(raw - min) / (max - min);

            return value;
        }
        public static int SetDouble(double value, int min, int max)
        {
            if(value > 1.0)
                value = 1.0;
            else if(value < 0.0)
                value = 0.0;
            else
            { }
            double raw = value * (max - min) + min;
            return (int)Math.Round(raw);
        }

        public static byte GetByte(int raw, byte defaultValue)
        {
            if (raw < byte.MinValue || raw > byte.MaxValue)
                return defaultValue;
            return (byte)raw;
        }
    }
}
