using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    partial class Rovio
    {
        static void GetJoystickAngle(double axisX, double axisY, out double angle, out double value)
        {
            double radius;//半径
            double radian;//弧度，相对X轴的夹角
            if (axisX == 0 && axisY == 0)
            {
                radius = 0;
                radian = 0;
            }
            else
            {
                radius = Math.Sqrt(axisX * axisX + axisY * axisY);
                radian = Math.Atan2(axisY, axisX);
            }

            angle = radian / Math.PI * 180.0 + 90;
            angle = (angle + 720) % 360;

            value = Math.Sqrt(axisX * axisX + axisY * axisY);
        }
    }
}
