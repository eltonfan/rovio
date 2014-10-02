using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver.Actions
{
    public class SetHeadLight : BaseAction
    {
        readonly HeadLightState state = HeadLightState.Off;
        public SetHeadLight(HeadLightState state)
        {
            this.state = state;
        }

        public override void Execute()
        {
            RovioAPI api = rovio.API;
            if (api == null)
                throw new Exception("Rovio 尚未连接。");

            rovio.Settings.SetHeadLight(this.state);
        }
    }
}
