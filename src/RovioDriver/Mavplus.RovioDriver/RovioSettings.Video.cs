using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver
{
    partial class RovioSettings
    {
        public class VideoGroup : SettingGroup
        {
            byte brightness = 0;
            /// <summary>
            /// Indicates the current brightness level
            /// 
            /// 1 (dimmest) – 6 (brightest)
            /// </summary>
            public byte Brightness
            {
                get { return this.brightness; }
                set
                {
                    if (this.brightness == value)
                        return;
                    this.brightness = value;

                    this.modified = true;
                }
            }

            int resolutionId = 0;
            public int ResolutionId
            {
                get { return this.resolutionId; }
                set
                {
                    if (this.resolutionId == value)
                        return;
                    this.resolutionId = value;

                    this.modified = true;
                }
            }

            VideoCompression videoCompression = VideoCompression.Med;
            /// <summary>
            /// Video compression
            /// </summary>
            public VideoCompression VideoCompression
            {
                get { return this.videoCompression; }
                set
                {
                    if (this.videoCompression == value)
                        return;
                    this.videoCompression = value;

                    this.modified = true;
                }
            }

            int frameRate = 0;
            /// <summary>
            /// Frame rate.
            /// 
            /// 1 - 30
            /// </summary>
            public int FrameRate
            {
                get { return this.frameRate; }
                set
                {
                    if (this.frameRate == value)
                        return;
                    this.frameRate = value;

                    this.modified = true;
                }
            }

            ACFrequency acFrequency = ACFrequency.AutoDetect;
            /// <summary>
            /// Projector’s frequency
            /// </summary>
            public ACFrequency ACFrequency
            {
                get { return this.acFrequency; }
                set
                {
                    if (this.acFrequency == value)
                        return;
                    this.acFrequency = value;

                    this.modified = true;
                }
            }

            readonly RovioSettings owner = null;
            internal VideoGroup(RovioSettings owner)
            {
                this.owner = owner;
            }

            internal override void Load(Dictionary<FlashParameters, Int32> flashParameters, RovioStatusReport report, RovioMcuReport mcuReport)
            {
                RovioAPI api = owner.rovio.API;

                this.brightness = report.brightness;
                this.resolutionId = report.ResolutionId;
                this.videoCompression = report.VideoCompression;
                this.frameRate = report.frame_rate;
                this.acFrequency = report.ACFrequency;

                this.modified = false;
            }

            public override void Save()
            {
                if (!this.modified)
                    return;

                RovioAPI api = owner.rovio.API;
                CameraController camera = api.Camera;

                camera.ChangeBrightness(this.brightness);
                camera.ChangeResolution(this.resolutionId);
                camera.ChangeCompressRatio(this.videoCompression);
                camera.ChangeFramerate(this.frameRate);
                camera.SetCamera(this.acFrequency);

                this.modified = false;
            }



            /// <summary>
            /// 0=[176x144] 1=[320x240] 2=[352x240] 3=[640x480]
            /// </summary>
            static readonly Size[] resolutions = new Size[] {
                new Size(176, 144),
                new Size(320, 240),
                new Size(352, 240),
                new Size(640, 480),
            };

            public static Size[] AvailableResolutions
            {
                get { return resolutions; }
            }

            /// <summary>
            /// Resolution
            /// </summary>
            public Size Resolution
            {
                get
                {
                    if (this.ResolutionId < 0 || this.ResolutionId > resolutions.Length - 1)
                        return Size.Empty;
                    return resolutions[this.ResolutionId];
                }
            }
        }
    }
}
