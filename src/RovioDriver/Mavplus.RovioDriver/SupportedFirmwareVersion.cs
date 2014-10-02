using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Mavplus.RovioDriver
{
    public enum SupportedFirmwareVersion
    {
        [Description("v1.3.3及以下版本")]
        Firmware_133_Or_Below,
        [Description("v1.5.x到v1.6.4")]
        Firmware_Between_15x_And_164,
        [Description("v1.6.4及以上版本")]
        Firmware_164_Or_Above,
    }
}
