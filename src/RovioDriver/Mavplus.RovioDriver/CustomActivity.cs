using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Mavplus.RovioDriver
{
    public enum ActivityState
    {
        Normal,
        Checked,
        Disabled,
    }
    public abstract class CustomActivity
    {
        public IWin32Window owner { get; set; }
        public IRovio Rovio { get; set; }

        ActivityState state = ActivityState.Normal;
        public ActivityState State
        {
            get { return this.state; }
            set
            {
                if (this.state == value)
                    return;
                this.state = value;
                if (this.StateChanged != null)
                    this.StateChanged(this, EventArgs.Empty);
            }
        }
        public event EventHandler StateChanged;

        public Bitmap Icon { get; private set; }
        public string Caption { get; private set; }
        public string Description { get; private set; }

        protected CustomActivity(Bitmap icon, string caption, string desc)
        {
            this.Icon = icon;
            this.Caption = caption;
            this.Description = desc;
        }

        public void Execute(INotifier notifier)
        {
            if (this.Rovio == null)
                throw new ArgumentNullException("Rovio", "Rovio 不能为空。");

            this.ExecuteInternal(this.Rovio, notifier);
        }

        protected abstract void ExecuteInternal(IRovio rovio, INotifier notifier);
    }
}
