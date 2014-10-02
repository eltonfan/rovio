using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver
{
    /// <summary>
    /// 运动控制：手动驾驶功能。
    /// </summary>
    internal class ManualDriver : IRovioManualDriver
    {
        readonly MovementController rovio = null;
        public ManualDriver(MovementController rovio)
        {
            this.rovio = rovio;
        }

        bool isrunning = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="speed">速度 (0.0, 1.0]</param>
        /// <param name="angle">旋转角度，[0, 180]</param>
        public bool ManualDrive(Actions action, double speed, int? angle = null)
        {
            return ManualDrive(new DriveArguments(action, speed, angle));
        }
        public bool ManualDrive(DriveArguments arguments)
        {
            if (isrunning)
                return false;

            isrunning = true;
            try
            {
                //s_value = 1 (fastest) – 10 (slowest) 
                int speedTick = RovioAPI.SetDouble(arguments.Speed, 10, 1);
                int? angleTick = null;
                if(arguments.Angle == null)
                    angleTick = null;
                else
                    angleTick = (int)Math.Floor(arguments.Angle.Value / 12.0);
                //angleTick 务必小于32，否则会溢出导致不断转圈。
                rovio.ManualDrive((int)arguments.Action, speedTick, angleTick);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                isrunning = false;
            }

            return true;
        }

        /// <summary>
        /// 停止当前动作。
        /// </summary>
        public void Stop()
        {
            ManualDrive(Actions.Stop, 1.0);
        }
        /// <summary>
        /// 前进。
        /// </summary>
        /// <param name="speed"></param>
        public void Forward(double speed = 1.0)
        {
            ManualDrive(Actions.Forward, speed);
        }
        /// <summary>
        /// 后退。
        /// </summary>
        /// <param name="speed"></param>
        public void Backward(double speed = 1.0)
        {
            ManualDrive(Actions.Backward, speed);
        }
        /// <summary>
        /// 向左移动。
        /// </summary>
        /// <param name="speed"></param>
        public void StraightLeft(double speed = 1.0)
        {
            ManualDrive(Actions.StraightLeft, speed);
        }
        /// <summary>
        /// 向右移动。
        /// </summary>
        /// <param name="speed"></param>
        public void StraightRight(double speed = 1.0)
        {
            ManualDrive(Actions.StraightRight, speed);
        }
        /// <summary>
        /// 左转。
        /// </summary>
        /// <param name="speed"></param>
        public void RotateLeft(double speed = 1.0)
        {
            ManualDrive(Actions.RotateLeftBySpeed, speed);
        }
        /// <summary>
        /// 右转。
        /// </summary>
        /// <param name="speed"></param>
        public void RotateRight(double speed = 1.0)
        {
            ManualDrive(Actions.RotateRightBySpeed, speed);
        }
        /// <summary>
        /// 向左前方移动。
        /// </summary>
        /// <param name="speed"></param>
        public void DiagonalForwardLeft(double speed = 1.0)
        {
            ManualDrive(Actions.DiagonalForwardLeft, speed);
        }
        /// <summary>
        /// 向右前方移动。
        /// </summary>
        /// <param name="speed"></param>
        public void DiagonalForwardRight(double speed = 1.0)
        {
            ManualDrive(Actions.DiagonalForwardRight, speed);
        }
        /// <summary>
        /// 向左后方移动。
        /// </summary>
        /// <param name="speed"></param>
        public void DiagonalBackwardLeft(double speed = 1.0)
        {
            ManualDrive(Actions.DiagonalBackwardLeft, speed);
        }
        /// <summary>
        /// 向右后方移动。
        /// </summary>
        /// <param name="speed"></param>
        public void DiagonalBackwardRight(double speed = 1.0)
        {
            ManualDrive(Actions.DiagonalBackwardRight, speed);
        }
        /// <summary>
        /// 移动摄像头到最高位置。
        /// </summary>
        public void HeadUp()
        {
            ManualDrive(Actions.HeadUp, 1.0);
        }
        /// <summary>
        /// 移动摄像头到最低位置。
        /// </summary>
        public void HeadDown()
        {
            ManualDrive(Actions.HeadDown, 1.0);
        }
        /// <summary>
        /// 移动摄像头到中间位置。
        /// </summary>
        public void HeadMiddle()
        {
            ManualDrive(Actions.HeadMiddle, 1.0);
        }
        /// <summary>
        /// 摄像头上移一点点。
        /// </summary>
        public void HeadUpward()
        {
            ManualDrive(Actions.HeadUp, 0.1);
            ManualDrive(Actions.Stop, 1.0);
        }
        /// <summary>
        /// 摄像头下移一点点。
        /// </summary>
        public void HeadDownward()
        {
            ManualDrive(Actions.HeadDown, 0.1);
            ManualDrive(Actions.Stop, 1.0);
        }
        /// <summary>
        /// 左转指定角度（刻度：12度）。
        /// </summary>
        public void RotateLeftByDegree(int angle)
        {
            ////sendMCUCommand('114D4D000100534852540001000111' + angle + '' + (speed < 10 ? '0' + speed : speed) + '00');    
            //sendCommand("rev.cgi", "Cmd=nav&action=18&drive=17&speed=" + speed + "&angle=" + angle);

            ManualDrive(Actions.RotateLeftBy20DegreeAngleIncrements, 1.0, angle);
        }
        /// <summary>
        /// 右转指定角度（刻度：12度）。
        /// </summary>
        public void RotateRightByDegree(int angle)
        {
            ////sendMCUCommand('114D4D000100534852540001000112' + angle + '' + (speed < 10 ? '0' + speed : speed) + '00');
            //sendCommand("rev.cgi", "Cmd=nav&action=18&drive=18&speed=" + speed + "&angle=" + angle);

            ManualDrive(Actions.RotateRightBy20DegreeAngleIncrements, 1.0, angle);
        }


        /// <summary>
        /// 手动控制的动作枚举。
        /// </summary>
        public enum Actions
        {
            Stop = 0,
            Forward = 1,
            Backward = 2,
            StraightLeft = 3,
            StraightRight = 4,
            RotateLeftBySpeed = 5,
            RotateRightBySpeed = 6,
            DiagonalForwardLeft = 7,
            DiagonalForwardRight = 8,
            DiagonalBackwardLeft = 9,
            DiagonalBackwardRight = 10,
            HeadUp = 11,
            HeadDown = 12,
            HeadMiddle = 13,
            //Reserved = 14,
            //Reserved = 15,
            //Reserved = 16, 
            RotateLeftBy20DegreeAngleIncrements = 17,
            RotateRightBy20DegreeAngleIncrements = 18,
        }

        public class DriveArguments
        {
            public Actions Action { get; private set; }
            public double Speed { get; private set; }
            public int? Angle { get; private set; }
            public DriveArguments(Actions action, double speed, int? angle = null)
            {
                this.Action = action;
                this.Speed = speed;
                this.Angle = angle;
            }
        }
    }
}
