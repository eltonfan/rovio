using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver.Actions
{
    /// <summary>
    /// 摄像头上移一点点。
    /// </summary>
    public class HeadUpward : BaseAction
    {
        public HeadUpward()
        { }

        public override void Execute()
        {
            RovioAPI api = rovio.API;
            if (api == null)
                throw new Exception("Rovio 尚未连接。");

            rovio.QueueWorkItem(api.ManualDriver.HeadUpward);
        }
    }
}
