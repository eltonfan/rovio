using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver.Actions
{
    public class SetNightMode : BaseAction
    {
        readonly NightMode mode = NightMode.Normal;
        public SetNightMode(NightMode mode)
        {
            this.mode = mode;
        }

        public override void Execute()
        {
            RovioAPI api = rovio.API;
            if (api == null)
                throw new Exception("Rovio 尚未连接。");

            rovio.Settings.SetNightMode(this.mode);
        }
    }
}
