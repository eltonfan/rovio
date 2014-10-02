using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver.Actions
{
    /// <summary>
    /// 恢复出厂设置。
    /// </summary>
    public class RestoreFactoryDefault : BaseAction
    {
        public RestoreFactoryDefault()
        { }

        public override void Execute()
        {
            RovioAPI api = rovio.API;
            if (api == null)
                throw new Exception("Rovio 尚未连接。");

            api.SetFactoryDefault();
        }
    }
}
