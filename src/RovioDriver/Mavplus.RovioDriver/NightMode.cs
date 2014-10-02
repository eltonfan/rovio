using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    /// <summary>
    /// 夜间模式。
    /// </summary>
    public enum NightMode : byte
    {
        /// <summary>
        /// 夜间模式
        /// </summary>
        Night = 0x58,
        Middle = 0x38,
        /// <summary>
        /// 普通模式。
        /// </summary>
        Normal = 0x18,
    }
}
