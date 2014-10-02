using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    public class StatusChangedEventArgs : EventArgs
    {
        public RovioStatusReport Data { get; private set; }
        public StatusChangedEventArgs(RovioStatusReport report)
        {
            this.Data = report;
        }
    }
}
