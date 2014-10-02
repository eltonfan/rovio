using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver.Actions
{
    public class SetBlueLights : BaseAction
    {
        readonly BlueLightState state = BlueLightState.None;
        public SetBlueLights(BlueLightState state)
        {
            this.state = state;
        }

        public override void Execute()
        {
            RovioAPI api = rovio.API;
            if (api == null)
                throw new Exception("Rovio 尚未连接。");

            rovio.Settings.SetBlueLight(this.state);
        }
    }
}
