using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    /// <summary>
    /// 设备的一个动作定义。
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// 执行这个动作。
        /// </summary>
        void Execute();
    }
}
