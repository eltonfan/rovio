using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver.Actions
{
    /// <summary>
    /// 一种抽象类，用于创建设备的动作。
    /// </summary>
    public abstract class BaseAction : IAction
    {
        /// <summary>
        /// 执行的设备引用。
        /// </summary>
        protected Rovio rovio = null;

        protected BaseAction()
        { }

        public virtual void Execute()
        {
        }

        /// <summary>
        /// 获取或设置 执行的设备引用。
        /// </summary>
        /// <value>
        /// 执行的设备引用。
        /// </value>
        public Rovio Rovio
        {
            get { return this.rovio; }
            set
            {
                this.rovio = value;
            }
        }
    }
}
