using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver
{
    partial class RovioSettings
    {
        public class MovementGroup : SettingGroup
        {
            double movementSpeed = 0.0;
            /// <summary>
            /// 移动速度。
            /// </summary>
            public double MovementSpeed
            {
                get { return this.movementSpeed; }
                set
                {
                    if (this.movementSpeed == value)
                        return;
                    this.movementSpeed = value;

                    this.modified = true;
                }
            }

            double turnSpeed = 0.0;
            /// <summary>
            /// 旋转的速度
            /// </summary>
            public double TurnSpeed
            {
                get { return this.turnSpeed; }
                set
                {
                    if (this.turnSpeed == value)
                        return;
                    this.turnSpeed = value;

                    this.modified = true;
                }
            }

            double rotSpeed = 0.0;
            /// <summary>
            /// RotSpeed
            /// </summary>
            public double AngleTurnSpeed
            {
                get { return this.rotSpeed; }
                set
                {
                    if (this.rotSpeed == value)
                        return;
                    this.rotSpeed = value;

                    this.modified = true;
                }
            }
            
            readonly RovioSettings owner = null;
            internal MovementGroup(RovioSettings owner)
            {
                this.owner = owner;
            }

            internal override void Load(Dictionary<FlashParameters, Int32> flashParameters, RovioStatusReport report, RovioMcuReport mcuReport)
            {
                //此处跟Rovio Web设置保持一致，范围是1-10, 1为最慢
                this.movementSpeed = RovioAPI.GetDouble(
                    flashParameters[FlashParameters.MovementSpeed], 1, 10, 0.5);
                this.turnSpeed = RovioAPI.GetDouble(
                    flashParameters[FlashParameters.TurnSpeed], 1, 10, 0.5);
                this.rotSpeed = RovioAPI.GetDouble(
                    flashParameters[FlashParameters.RotSpeed], 1, 10, 0.8);

                this.modified = false;
            }

            public override void Save()
            {
                if (!this.modified)
                    return;
                
                MovementController movement = owner.rovio.API.Movement;
                movement.SaveParameter(
                    new FlashParameterItem(FlashParameters.MovementSpeed, RovioAPI.SetDouble(this.movementSpeed, 1, 10)),
                    new FlashParameterItem(FlashParameters.TurnSpeed, RovioAPI.SetDouble(this.turnSpeed, 1, 10)),
                    new FlashParameterItem(FlashParameters.RotSpeed, RovioAPI.SetDouble(this.rotSpeed, 1, 10)));

                this.modified = false;
            }
        }
    }
}
