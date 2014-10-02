using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver.Actions
{
    public class MoveForward : BaseAction
    {
        readonly double speed = 0.0;
        public MoveForward(double speed)
        {
            this.speed = speed;
        }

        public override void Execute()
        {
            RovioAPI api = rovio.API;
            if (api == null)
                throw new Exception("Rovio 尚未连接。");

            api.ManualDriver.Forward(this.speed);
        }
    }
}
