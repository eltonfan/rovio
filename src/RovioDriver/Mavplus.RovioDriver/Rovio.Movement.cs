using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Threading;

namespace Mavplus.RovioDriver
{
    partial class Rovio
    {
        /// <summary>
        /// 控制指令重复的时间间隔。
        /// </summary>
        public const int MOVEMENT_INTERVAL = 100;

        readonly System.Timers.Timer timerMovement = new System.Timers.Timer();
        void InitMovement()
        {
            this.timerMovement.Interval = MOVEMENT_INTERVAL;
            this.timerMovement.Elapsed += new System.Timers.ElapsedEventHandler(timerMovement_Elapsed);
            this.timerMovement.AutoReset = false;
        }

        volatile float roll = 0.0F;
        volatile float pitch = 0.0F;
        volatile float yaw = 0.0F;
        volatile float gaz = 0.0F;

        public void SetFlightParameters(float roll, float pitch, float yaw, float gaz = 0.0F)
        {
            this.roll = roll;
            this.pitch = pitch;
            this.yaw = yaw;
            this.gaz = gaz;

            //如果是录制模式，则必须把相机放下。
            // lower head if recording
            //if(recording && !isClicked($('cam_down')))
            //{
            //    $('status').innerHTML = 'Lowering head to record';
            //    setTimeout("$('status').innerHTML = 'Recording';", 4000);
            //    setCamPosTo('down');
            //    return;
            //}
        }

        public void StopMoving()
        {
            api.ManualDriver.Stop();
            //sendMCUCommand("114D4D000100534852540001000100000000");

            this.SetFlightParameters(0, 0, 0, 0);
        }

        void timerMovement_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                ManualDriver.Actions? action = null;
                double angle, speed;
                GetJoystickAngle(roll, pitch, out angle, out speed);

                if (speed == 0.0)
                {
                    action = null;
                }
                else if (angle < 22.5)
                {
                    action = ManualDriver.Actions.Forward;
                    speed = Math.Abs(pitch);
                }
                else if (angle < 67.5)
                {
                    action = ManualDriver.Actions.DiagonalForwardRight;
                }
                else if (angle < 112.5)
                {
                    action = ManualDriver.Actions.StraightRight;
                    speed = Math.Abs(roll);
                }
                else if (angle < 157.5)
                {
                    action = ManualDriver.Actions.DiagonalBackwardRight;
                }
                else if (angle < 202.5)
                {
                    action = ManualDriver.Actions.Backward;
                    speed = Math.Abs(pitch);
                }
                else if (angle < 247.5)
                {
                    action = ManualDriver.Actions.DiagonalBackwardLeft;
                }
                else if (angle < 292.5)
                {
                    action = ManualDriver.Actions.StraightLeft;
                    speed = Math.Abs(roll);
                }
                else if (angle < 337.5)
                {
                    action = ManualDriver.Actions.DiagonalForwardLeft;
                }
                else if (angle <= 360)
                {
                    action = ManualDriver.Actions.Forward;
                    speed = Math.Abs(pitch);
                }

                if (action != null && speed > 0.2)
                {
                    api.ManualDriver.ManualDrive(action.Value, speed);
                }

                if (Math.Abs(yaw) < 0.1)
                {
                }
                else if (yaw > 0)
                {
                    api.ManualDriver.RotateRight(Math.Abs(yaw));
                }
                else if (yaw < 0)
                {
                    api.ManualDriver.RotateLeft(Math.Abs(yaw));
                }

                if (Math.Abs(gaz) < 0.1)
                {
                }
                else if (gaz > 0)
                {
                    api.ManualDriver.HeadUpward();
                }
                else if (gaz < 0)
                {
                    api.ManualDriver.HeadDownward();
                }
            }
            catch (Exception ex)
            {
                //
            }
            finally
            {
                if(isOpen)
                    timerMovement.Start();
            }
        }

        public void QueueWorkItem(Amib.Threading.Action action)
        {
            threadPool.QueueWorkItem(action);
        }


        BackgroundWorker bwMoveTo = null;
        volatile int moveToTargetX = 0;
        volatile int moveToTargetY = 0;
        public void MoveToAsync(Point target)
        {
            if (bwMoveTo == null)
            {
                bwMoveTo = new BackgroundWorker();
                bwMoveTo.WorkerSupportsCancellation = true;
                bwMoveTo.WorkerReportsProgress = true;
                bwMoveTo.DoWork += new DoWorkEventHandler(bwMoveTo_DoWork);
                bwMoveTo.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwMoveTo_RunWorkerCompleted);
            }
            this.moveToTargetX = target.X;
            this.moveToTargetY = target.Y;

            if (bwMoveTo.IsBusy)
                return;

            bwMoveTo.RunWorkerAsync(target);
        }

        public void CancelMoveAsync()
        {
            if (bwMoveTo == null || !bwMoveTo.IsBusy)
                return;
            bwMoveTo.CancelAsync();
        }

        void bwMoveTo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //
        }

        void bwMoveTo_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            Point target = (Point)e.Argument;

            MoveTo(bw);
        }


        void MoveTo(BackgroundWorker bw)
        {
            while (true)
            {
                if (bw.CancellationPending)
                    break;

                RovioLocation oldLocation = this.GetLocation();

                int axisX = moveToTargetX - oldLocation.X;
                int axisY = moveToTargetY - oldLocation.Y;

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

                if (radius < 500)
                    break;

                double angle = radian / Math.PI * 180.0 + 90;
                double angleOffset = angle - oldLocation.Angle;
                angleOffset = angleOffset % 360;
                if (angleOffset > 180)
                    angleOffset -= 360;
                if (Math.Abs(angleOffset) < 15)
                {//不需要旋转
                    api.ManualDriver.Forward(0.3);

                    Thread.Sleep(200);
                }
                else if (angleOffset > 0)
                {//左转
                    api.ManualDriver.RotateLeftByDegree((int)angleOffset);


                    Thread.Sleep(2000);
                }
                else
                {//右转
                    api.ManualDriver.RotateRightByDegree((int)-angleOffset);

                    Thread.Sleep(2000);
                }
            }
        }
    }
}
