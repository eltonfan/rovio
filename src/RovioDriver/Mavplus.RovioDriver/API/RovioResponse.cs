using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver.API
{
    public class RovioResponseItem
    {
        public string Key { get; private set; }
        public string Value { get; private set; }
        public RovioResponseItem(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0} = {1}", this.Key, this.Value);
        }
    }

    /// <summary>
    /// 响应中包含多组KeyValue。Key可能重复。
    /// </summary>
    public class RovioResponse : IEnumerable<RovioResponseItem>
    {
        readonly List<RovioResponseItem> items = null;
        readonly string str = null;
        public RovioResponse(string str)
        {
            items = new List<RovioResponseItem>();

            this.str = str;
            if (!string.IsNullOrEmpty(str))
            {
                string[] lines = str.Split(new string[] { "\r\n", "\r", "\n", "|" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('=');
                    if (parts.Length < 2)
                        continue;
                    this.Add(parts[0].Trim(), parts[1].Trim());
                }
            }
        }

        public IEnumerator<RovioResponseItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public void Add(string key, string value)
        {
            items.Add(new RovioResponseItem(key, value));
        }

        public bool Remove(RovioResponseItem item)
        {
            return items.Remove(item);
        }

        public int RemoveAll(string key)
        {
            return items.RemoveAll(d => d.Key == key);
        }

        public bool ContainsKey(string key)
        {
            return (items.Find(d => d.Key == key) != null);
        }

        public override string ToString()
        {
            return this.str;
        }

        /// <summary>
        /// 获取第一个匹配项的值。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                RovioResponseItem item = items.Find(d => d.Key == key);
                if (item == null)
                    return null;

                return item.Value;
            }
        }
    }
}
