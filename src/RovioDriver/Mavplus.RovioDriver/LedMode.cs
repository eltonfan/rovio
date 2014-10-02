using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    /// <summary>
    /// 蓝灯控制模式。
    /// </summary>
    [Flags]
    public enum BlueLightState : byte
    {
        None = 0,
        Led0 = 0x01,
        Led1 = 0x02,
        Led2 = 0x04,
        Led3 = 0x08,
        Led4 = 0x10,
        Led5 = 0x20,
        All = Led0 | Led1 | Led2 | Led3 | Led4 | Led5,
    }
}
