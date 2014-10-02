using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    internal class FlashParameterItem
    {
        /// <summary>
        /// 0 – 19
        /// </summary>
        public FlashParameters Key { get; private set; }
        /// <summary>
        /// 32bit signed integer
        /// </summary>
        public Int32 Value { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">0 – 19</param>
        /// <param name="value">32bit signed integer</param>
        public FlashParameterItem(FlashParameters key, Int32 value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
