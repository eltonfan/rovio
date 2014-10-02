using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver
{
    public class RovioMcuReport
    {
        public HeadLightState HeadLight { get; set; }
        public static RovioMcuReport Parse(RovioResponse dic)
        {
            RovioMcuReport report = new RovioMcuReport();

            //0E0100000000000000000003CB744B
            string responses = dic["responses"];
            byte[] data = new byte[responses.Length / 2];
            for(int i=0;i<data.Length; i++)
                data[i] = byte.Parse(responses.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);

            int offset = 0;
            //Length of the packet
            int length = data[offset]; offset += 1;
            //NOT IN USE
            offset += 1;
            //Direction of rotation of left wheel since last read (bit 2)
            int leftWheelDirection = data[offset]; offset +=1;
            //Number of left wheel encoder ticks since last read
            int leftWheelTicks = data[offset] + data[offset + 1] * 256; offset += 2;
            //Direction of rotation of right wheel since last read (bit 2)
            int rightWheelDirection = data[offset]; offset +=1;
            //Number of right wheel encoder ticks since last read
            int rightWheelTicks = data[offset] + data[offset + 1] * 256; offset += 2;
            //Direction of rotation of rear wheel since last read (bit 2)
            int rearWheelDirection = data[offset]; offset +=1;
            //Number of rear wheel encoder ticks since last read
            int rearWheelTicks = data[offset] + data[offset + 1] * 256; offset += 2;
            //NOT IN USE
            offset += 1;
            //Head position
            int headPosition = data[offset]; offset += 1;
            //Battery
            //  0x7F: Battery Full (0x7F or higher for new battery)
            //  0x??: Orange light in Rovio head. ( to be define)
            //  0x6A: Very low battery (Hungry, danger, very low battery level)
            //  libNS need take control to go home and charging
            //  0x64: Shutdown level (MCU will cut off power for protecting the battery)
            int battery = data[offset]; offset += 1;
            //status
            //bit 0 : Light LED (head) status, 0: OFF, 1: ON
            //bit 1 : IR-Radar power status. 0: OFF, 1: ON
            //bit 2 : IR-Radar detector status: 0: fine, 1: barrier detected.
            //bit 3-5: Charger staus
            //        0x00 : nothing happen
            //        0x01 : charging completed.
            //        0x02 : in charging
            //        0x04 : something wrong, error occur.
            //bit 6,7: undefined, do not use.
            byte status = data[offset]; offset += 1;

            report.HeadLight = ((status & 0x01) == 0x01) ? HeadLightState.On : HeadLightState.Off;

            return report;
        }
    }
}
