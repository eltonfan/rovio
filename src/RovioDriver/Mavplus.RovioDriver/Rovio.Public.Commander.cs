using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    partial class Rovio
    {
        public void StartEngines()
        {
            this.Execute(new Actions.HeadUp());
        }

        public void StopEngines()
        {
        }

        public void StartReset()
        {
        }

        public void StopReset()
        {
        }

        public bool StartRecordVideo()
        {
            return true;
        }

        public void StopRecordVideo()
        {
        }

        public bool TakePicture()
        {
            return true;
        }


        public void PlayLedAnimation(int id, int frequency, int duration)
        {
        }

        public void DisplayNextVideoChannel()
        {
        }

        public void SwitchVideoChannel(int videoChannel)
        {
        }

        public void SetTakePicture()
        {
        }

        public void StartVisionDetect()
        {
        }

        public void StopVisionDetect()
        {
        }
    }
}
