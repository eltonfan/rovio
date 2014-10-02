using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver.Actions
{
    /// <summary>
    /// </summary>
    public class GoHomeAndDock : BaseAction
    {
        public GoHomeAndDock()
        { }

        public override void Execute()
        {
            RovioAPI api = rovio.API;
            if (api == null)
                throw new Exception("Rovio 尚未连接。");

            rovio.QueueWorkItem(api.Movement.GoHomeAndDock);
        }
    }
}
