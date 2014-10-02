using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver.Actions
{
    /// <summary>
    /// 左转指定角度（刻度：12度）。
    /// </summary>
    public class RotateLeftByDegree : BaseAction
    {
        readonly int angle = 0;
        public RotateLeftByDegree(int angle)
        {
            this.angle = angle;
        }

        public override void Execute()
        {
            RovioAPI api = rovio.API;
            if (api == null)
                throw new Exception("Rovio 尚未连接。");

            api.ManualDriver.RotateLeftByDegree(angle);
        }
    }
}
