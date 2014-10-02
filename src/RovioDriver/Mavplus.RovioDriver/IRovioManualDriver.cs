using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    public interface IRovioManualDriver
    {
        /// <summary>
        /// 停止当前动作。
        /// </summary>
        void Stop();
        /// <summary>
        /// 前进。
        /// </summary>
        /// <param name="speed"></param>
        void Forward(double speed = 1.0);
        /// <summary>
        /// 后退。
        /// </summary>
        /// <param name="speed"></param>
        void Backward(double speed = 1.0);
        /// <summary>
        /// 向左移动。
        /// </summary>
        /// <param name="speed"></param>
        void StraightLeft(double speed = 1.0);
        /// <summary>
        /// 向右移动。
        /// </summary>
        /// <param name="speed"></param>
        void StraightRight(double speed = 1.0);
        /// <summary>
        /// 左转。
        /// </summary>
        /// <param name="speed"></param>
        void RotateLeft(double speed = 1.0);
        /// <summary>
        /// 右转。
        /// </summary>
        /// <param name="speed"></param>
        void RotateRight(double speed = 1.0);
        /// <summary>
        /// 向左前方移动。
        /// </summary>
        /// <param name="speed"></param>
        void DiagonalForwardLeft(double speed = 1.0);
        /// <summary>
        /// 向右前方移动。
        /// </summary>
        /// <param name="speed"></param>
        void DiagonalForwardRight(double speed = 1.0);
        /// <summary>
        /// 向左后方移动。
        /// </summary>
        /// <param name="speed"></param>
        void DiagonalBackwardLeft(double speed = 1.0);
        /// <summary>
        /// 向右后方移动。
        /// </summary>
        /// <param name="speed"></param>
        void DiagonalBackwardRight(double speed = 1.0);
        /// <summary>
        /// 移动摄像头到最高位置。
        /// </summary>
        void HeadUp();
        /// <summary>
        /// 移动摄像头到最低位置。
        /// </summary>
        void HeadDown();
        /// <summary>
        /// 移动摄像头到中间位置。
        /// </summary>
        void HeadMiddle();
        /// <summary>
        /// 摄像头上移一点点。
        /// </summary>
        void HeadUpward();
        /// <summary>
        /// 摄像头下移一点点。
        /// </summary>
        void HeadDownward();
        /// <summary>
        /// 左转指定角度（刻度：12度）。
        /// </summary>
        void RotateLeftByDegree(int angle);
        /// <summary>
        /// 右转指定角度（刻度：12度）。
        /// </summary>
        void RotateRightByDegree(int angle);
    }
}
