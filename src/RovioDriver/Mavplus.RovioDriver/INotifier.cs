using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    public interface INotifier
    {
        void SetCaption(string caption);
        void SetDescription(string description);
    }
}
