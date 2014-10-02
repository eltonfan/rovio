using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver.API
{
    /// <summary>
    /// 请求命令参数。
    /// </summary>
    public class RequestItem
    {
        public string Key { get; private set; }
        public string Value { get; private set; }
        public RequestItem(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
        public RequestItem(string key, bool value)
            : this(key, value.ToString())
        { }
        public RequestItem(string key, int value)
            : this(key, value.ToString())
        { }
        public RequestItem(string key, long value)
            : this(key, value.ToString())
        { }
        public RequestItem(string key, double value)
            : this(key, value.ToString())
        { }
    }
}
