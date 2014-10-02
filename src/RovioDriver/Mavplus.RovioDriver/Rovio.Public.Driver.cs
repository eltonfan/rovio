using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    partial class Rovio : IRovioManualDriver
    {
        public void Stop()
        {
            api.ManualDriver.Stop();
        }

        public void Forward(double speed = 1.0)
        {
            api.ManualDriver.Forward(speed);
        }

        public void Backward(double speed = 1.0)
        {
            api.ManualDriver.Backward(speed);
        }

        public void StraightLeft(double speed = 1.0)
        {
            api.ManualDriver.StraightLeft(speed);
        }

        public void StraightRight(double speed = 1.0)
        {
            api.ManualDriver.StraightRight(speed);
        }

        public void RotateLeft(double speed = 1.0)
        {
            api.ManualDriver.RotateLeft(speed);
        }

        public void RotateRight(double speed = 1.0)
        {
            api.ManualDriver.RotateRight(speed);
        }

        public void DiagonalForwardLeft(double speed = 1.0)
        {
            api.ManualDriver.DiagonalForwardLeft(speed);
        }

        public void DiagonalForwardRight(double speed = 1.0)
        {
            api.ManualDriver.DiagonalForwardRight(speed);
        }

        public void DiagonalBackwardLeft(double speed = 1.0)
        {
            api.ManualDriver.DiagonalBackwardLeft(speed);
        }

        public void DiagonalBackwardRight(double speed = 1.0)
        {
            api.ManualDriver.DiagonalBackwardRight(speed);
        }

        public void HeadUp()
        {
            api.ManualDriver.HeadUp();
        }

        public void HeadDown()
        {
            api.ManualDriver.HeadDown();
        }

        public void HeadMiddle()
        {
            api.ManualDriver.HeadMiddle();
        }

        public void HeadUpward()
        {
            api.ManualDriver.HeadUpward();
        }

        public void HeadDownward()
        {
            api.ManualDriver.HeadDownward();
        }

        public void RotateLeftByDegree(int angle)
        {
            api.ManualDriver.RotateLeftByDegree(angle);
        }

        public void RotateRightByDegree(int angle)
        {
            api.ManualDriver.RotateRightByDegree(angle);
        }
    }
}
