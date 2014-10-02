using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    /// <summary>
    /// parameter indexes
    /// </summary>
    internal enum FlashParameters : int
    {
        /// <summary>
        /// i_MR 保持图像的原始比例。
        /// </summary>
        MaintainAspectRatio = 0,
        /// <summary>
        /// i_MS 移动速度。Movement Speed
        /// </summary>
        MovementSpeed = 1,
        /// <summary>
        /// i_TS 旋转的速度。Turn Speed
        /// </summary>
        TurnSpeed = 2,
        /// <summary>
        /// i_RS Angle Turn Speed
        /// </summary>
        RotSpeed = 3,
        /// <summary>
        /// i_LR
        /// </summary>
        latency = 4,
        /// <summary>
        /// i_SVP   safari video player
        /// </summary>
        video_player = 5,
        /// <summary>
        /// i_UPnP  upnp just enabled
        /// </summary>
        upnp_just_enabled = 6,
        /// <summary>
        /// i_SS    show status
        /// </summary>
        show_online_status = 7,
        /// <summary>
        /// i_MIIP1  manual ip
        /// </summary>
        manual_internetip = 8,
        /// <summary>
        /// i_MIIP2   manual ip
        /// </summary>
        manual_internetip2 = 9,
        /// <summary>
        /// i_VIA    verify internet access
        /// </summary>
        net_verify_access = 10,
        /// <summary>
        /// i_NFA 固件更新提示。Alert me when new firmware is available
        /// 是则为1  否则为0
        /// </summary>
        firmware_alert = 11,
        /// <summary>
        /// i_AVF    auto set video frequency
        /// </summary>
        video_freq = 12,

        /// <summary>
        /// 蓝灯控制设置。
        /// </summary>
        BlueLights = 18,
        /// <summary>
        /// 夜间模式设置，1 byte
        /// </summary>
        NightMode = 19,
    }
}