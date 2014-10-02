using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver.Actions
{
    /// <summary>
    /// 移动摄像头到中间位置。
    /// </summary>
    public class HeadMiddle : BaseAction
    {
        public HeadMiddle()
        { }

        public override void Execute()
        {
            RovioAPI api = rovio.API;
            if (api == null)
                throw new Exception("Rovio 尚未连接。");

            rovio.QueueWorkItem(api.ManualDriver.HeadMiddle);
        }
    }
}
