using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    partial class Rovio : IRovio
    {
        public void Execute(params IAction[] actions)
        {
            foreach (IAction item in actions)
            {
                Actions.BaseAction baseAction = item as Actions.BaseAction;
                if (baseAction != null)
                    baseAction.Rovio = this;

                item.Execute();
            }
        }

        public System.Drawing.Bitmap GetImage()
        {
            if (api == null)
                throw new Exception("Rovio 尚未连接。");
            return api.Camera.GetImage();
        }

        public HeadLightState HeadLight
        {
            get { return settings.HeadLight; }
            set
            {
                if (settings.HeadLight == value)
                    return;
                settings.SetHeadLight(value);
            }
        }


        public RovioLocation GetLocation()
        {
            if (api == null)
                throw new Exception("Rovio 尚未连接。");
            return api.Movement.GetReport().Location;
        }

        public void Emergency()
        {
            CancelMoveAsync();

            this.Execute(new Actions.Stop());
        }
    }
}
