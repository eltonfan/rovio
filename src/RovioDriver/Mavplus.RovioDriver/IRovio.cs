using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Mavplus.RovioDriver
{
    public partial interface IRovio : IRovioManualDriver
    {
        void Execute(params IAction[] actions);
        
        /// <summary>
        /// The basic command for acquiring Image.
        /// </summary>
        /// <returns>Bitmap</returns>
        Bitmap GetImage();

        HeadLightState HeadLight { get; set; }

        RovioLocation GetLocation();

        void Emergency();
    }
}
