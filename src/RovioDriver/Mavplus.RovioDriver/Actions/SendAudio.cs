using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver.Actions
{
    public class SendAudio : BaseAction
    {
        readonly byte[] audio = null;
        readonly int offset = 0;
        readonly int length = 0;
        public SendAudio(byte[] audio, int offset, int length)
        {
            this.audio = audio;
            this.offset = offset;
            this.length = length;
        }
        public SendAudio(byte[] audio)
            : this(audio, 0, audio.Length)
        { }

        public override void Execute()
        {
            RovioAPI api = rovio.API;
            if (api == null)
                throw new Exception("Rovio 尚未连接。");

            api.GetAudio(audio, offset, length);
        }
    }
}
