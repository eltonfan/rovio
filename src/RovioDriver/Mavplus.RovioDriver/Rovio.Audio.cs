using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Mavplus.RovioDriver
{
    partial class Rovio
    {
        volatile bool audioSending = false;

        BackgroundWorker bwSendAudio = null;
        public void SendAudioAsync(byte[] audio)
        {
            if (api == null)
                throw new Exception("Rovio 尚未连接。");

            if (audioSending)
                return;
            audioSending = true;

            if (bwSendAudio == null)
            {
                bwSendAudio = new BackgroundWorker();
                bwSendAudio.WorkerSupportsCancellation = true;
                bwSendAudio.WorkerReportsProgress = false;
                bwSendAudio.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSendAudio_RunWorkerCompleted);
                bwSendAudio.DoWork += new DoWorkEventHandler(bwSendAudio_DoWork);
            }
            try
            {
                bwSendAudio.RunWorkerAsync(audio);
            }
            catch (Exception ex)
            {
                audioSending = false;
            }
        }

        public void CancelSendAudioAsync()
        {
            if (bwSendAudio == null)
                return;
            if (!bwSendAudio.IsBusy)
                return;
            bwSendAudio.CancelAsync();
        }

        void bwSendAudio_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            byte[] audio = e.Argument as byte[];

            api.GetAudio(audio, 0, audio.Length, bw);
        }

        void bwSendAudio_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.audioSending = false;
            if (this.SendAudioCompleted != null)
                this.SendAudioCompleted(this, e);
        }

        public bool AudioSending
        {
            get { return this.audioSending; }
        }

        public event EventHandler SendAudioCompleted;
    }
}
