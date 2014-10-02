using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver.Actions
{
    /// <summary>
    /// 摄像头下移一点点。
    /// </summary>
    public class HeadDownward : BaseAction
    {
        public HeadDownward()
        { }

        public override void Execute()
        {
            RovioAPI api = rovio.API;
            if (api == null)
                throw new Exception("Rovio 尚未连接。");

            rovio.QueueWorkItem(api.ManualDriver.HeadDownward);
        }
    }
}
