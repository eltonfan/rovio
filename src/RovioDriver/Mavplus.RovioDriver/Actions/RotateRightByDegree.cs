using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver.Actions
{
    /// <summary>
    /// 右转指定角度（刻度：12度）。
    /// </summary>
    public class RotateRightByDegree : BaseAction
    {
        readonly int angle = 0;
        public RotateRightByDegree(int angle)
        {
            this.angle = angle;
        }

        public override void Execute()
        {
            RovioAPI api = rovio.API;
            if (api == null)
                throw new Exception("Rovio 尚未连接。");

            api.ManualDriver.RotateRightByDegree(angle);
        }
    }
}
