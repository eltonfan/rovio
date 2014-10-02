using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Mavplus.RovioDriver.API;

namespace Mavplus.RovioDriver
{
    /// <summary>
    /// 运动控制方法。
    /// </summary>
    internal class MovementController
    {
        readonly RovioAPI rovio = null;
        readonly ManualDriver driver = null;
        readonly RovioStatusReport report = new RovioStatusReport();
        public MovementController(RovioAPI rovio)
        {
            this.rovio = rovio;
            this.driver = new ManualDriver(this);
        }

        /// <summary>
        /// 运动控制指令。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>、
        internal RovioResponse MovementControl(Commands value, params RequestItem[] parameters)
        {
            List<RequestItem> list = new List<RequestItem>();
            list.Add(new RequestItem("Cmd", "nav"));
            list.Add(new RequestItem("action", (int)value));
            if (parameters != null)
                list.AddRange(parameters);

            return rovio.Request("/rev.cgi", list.ToArray());
        }

        /// <summary>
        /// Generates a report from libNS module that provides Rovio’s current status.
        /// </summary>
        /// <returns></returns>
        public RovioStatusReport GetReport()
        {
            RovioResponse reports = MovementControl(Commands.GetReport);
            ResponseCodes responses = (ResponseCodes)int.Parse(reports["responses"]);
            if(responses != ResponseCodes.SUCCESS)
                throw new Exception("获取报告失败：" + responses);

            report.Update(reports);
            return report;
        }

        /// <summary>
        /// Start recording a path.
        /// </summary>
        /// <returns></returns>
        public string StartRecoding()
        {
            RovioResponse response = MovementControl(Commands.StartRecoding);
            return "";
        }

        /// <summary>
        /// Terminates recording of a path without storing it to flash memory.
        /// </summary>
        /// <returns></returns>
        public string AbortRecording()
        {
            RovioResponse response = MovementControl(Commands.AbortRecording);
            return "";
        }

        /// <summary>
        /// Stops the recoding of a path and stores it in flash memory; javascript will give default name if user does not provide one.
        /// </summary>
        /// <param name="PathName">name of the path</param>
        /// <returns>Response code</returns>
        public string StopRecording(string PathName)
        {
            RovioResponse response = MovementControl(Commands.StopRecording,
                new RequestItem("name", PathName));
            return "";
        }

        /// <summary>
        /// Deletes specified path.
        /// </summary>
        /// <param name="PathName">name of the path</param>
        /// <returns>Response code</returns>
        public string Deletepath(string PathName)
        {
            RovioResponse response = MovementControl(Commands.Deletepath,
                new RequestItem("name", PathName));
            return "";
        }

        /// <summary>
        /// Returns a list of paths stored in the robot.
        /// </summary>
        /// <returns></returns>
        public string[] GetPathList()
        {
            RovioResponse response = MovementControl(Commands.GetPathList);

            // solves version bug
            if (response.ContainsKey("version"))
            {
                Thread.Sleep(2000);
                return this.GetPathList();
            }
            List<string> list = new List<string>();
            if (response["responses"] == "0")
            {
                //path_list_refreshed = 1;

                //if (list.indexOf(" = 0") != -1)
                //{
                //    list = list.substring(list.indexOf("responses = 0") + 13);
                //    if (trim(list).length)
                //    {
                //        var paths = list.split('|');
                //        for (i = 0; i < paths.length; i++)
                //        {
                //            addPathToList(paths[i]);
                //        }
                //    }
                //}

            }

            return list.ToArray();
        }

        /// <summary>
        /// Replays a stored path from closest point to the end; If the NorthStar signal is lost, it stops.
        /// </summary>
        /// <remarks>In API 1.2 there is no mention of PathName parameter</remarks>
        /// <param name="PathName"></param>
        /// <returns></returns>
        public string PlayPathForward(string PathName)
        {
            RovioResponse response = MovementControl(Commands.PlayPathForward,
                new RequestItem("name", PathName));
            return "";
        }

        /// <summary>
        /// Replays a stored path from closest point to the beginning; If NorthStar signal is lost it stops.
        /// </summary>
        /// <remarks>In API 1.2 there is no mention of PathName parameter</remarks>
        /// <param name="PathName"></param>
        /// <returns></returns>
        public string PlayPathBackward(string PathName)
        {
            RovioResponse response = MovementControl(Commands.PlayPathBackward,
                new RequestItem("name", PathName));
            return "";
        }

        /// <summary>
        /// Stop playing a path.
        /// </summary>
        /// <returns></returns>
        public string StopPlaying()
        {
            RovioResponse response = MovementControl(Commands.StopPlaying);
            return "";
        }

        /// <summary>
        /// Pause the robot and waits for a new pause or stop command.
        /// </summary>
        /// <returns></returns>
        public string PausePlaying()
        {
            RovioResponse response = MovementControl(Commands.PausePlaying);
            return "";
        }

        /// <summary>
        /// Rename the old path.
        /// </summary>
        /// <param name="OldPathName"></param>
        /// <param name="NewPathName"></param>
        /// <returns></returns>
        public string RenamePath(string OldPathName, string NewPathName)
        {
            RovioResponse response = MovementControl(Commands.RenamePath,
                new RequestItem("name", OldPathName),
                new RequestItem("newname", NewPathName));
            return "";
        }

        /// <summary>
        /// Drive to home location in front of charging station.
        /// </summary>
        /// <returns></returns>
        public string GoHome()
        {
            RovioResponse response = MovementControl(Commands.GoHome);
            return "";
        }

        /// <summary>
        /// Drive to home location in front of charging station and dock.
        /// </summary>
        /// <returns></returns>
        public void GoHomeAndDock()
        {
            RovioResponse response = MovementControl(Commands.GoHomeAndDock);
        }


        /// <summary>
        /// Define current position as home location in front of charging station.
        /// </summary>
        /// <returns></returns>
        public void UpdateHomePosition()
        {
            RovioResponse response = MovementControl(Commands.UpdateHomePosition);
        }

        /// <summary>
        /// Change homing, docking and driving parameters – speed for driving commands.
        /// </summary>
        /// <returns></returns>
        public void SetTuningParameters(TuningParameters value)
        {
            MovementControl(Commands.SetTuningParameters,
                new RequestItem("LeftRight", value.LeftRight),
                new RequestItem("Forward", value.Forward),
                new RequestItem("Reverse", value.Reverse),
                new RequestItem("DriveTurn", value.DriveTurn),
                new RequestItem("HomingTurn", value.HomingTurn),
                new RequestItem("ManDrive", value.ManDrive),
                new RequestItem("ManTurn", value.ManTurn),
                new RequestItem("DockTimeout", value.DockTimeout));
        }


        /// <summary>
        /// Returns homing, docking and driving parameters.
        /// </summary>
        /// <returns></returns>
        public TuningParameters GetTuningParameters()
        {
            RovioResponse response = MovementControl(Commands.GetTuningParameters);
            TuningParameters result = new TuningParameters
            {
                LeftRight = byte.Parse(response["LeftRight"]),
                Forward = byte.Parse(response["Forward"]),
                Reverse = byte.Parse(response["Reverse"]),
                DriveTurn = byte.Parse(response["DriveTurn"]),
                HomingTurn = byte.Parse(response["HomingTurn"]),
                ManDrive = byte.Parse(response["ManDrive"]),
                ManTurn = byte.Parse(response["ManTurn"]),
                DockTimeout = int.Parse(response["DockTimeout"]),
            };
            return result;
        }

        /// <summary>
        /// Stops whatever it was doing and resets to idle state.
        /// </summary>
        /// <returns></returns>
        public string ResetNavStateMachine()
        {
            RovioResponse response = MovementControl(Commands.ResetNavStateMachine);
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="speed">1 (fastest) – 10 (slowest) </param>
        internal string ManualDrive(int action, int speed, int? angle = null)
        {
            if (angle == null)
            {
                RovioResponse response = this.MovementControl(Commands.ManualDrive,
                    new RequestItem("drive", action),
                    new RequestItem("speed", speed));
            }
            else
            {
                RovioResponse response = this.MovementControl(Commands.ManualDrive,
                    new RequestItem("drive", action),
                    new RequestItem("speed", speed),
                    new RequestItem("angle", angle.Value));
            }
            return "";
        }

        /// <summary>
        ///  Turning on/off IR detector.
        /// </summary>
        /// <param name="state">0 - off, 1 - on</param>
        /// <returns></returns>
        public string SetIRState(bool enabled)
        {
            RovioResponse response = MovementControl(Commands.TestCommand,
                new RequestItem("IR", enabled ? 1 : 0));
            return "";
        }

        /// <summary>
        /// Turn off or turn on Rovio head light.
        /// </summary>
        /// <param name="Value">0 - Off, 1 - On</param>
        /// <returns></returns>
        public string SetHeadLight(HeadLightState state)
        {
            RovioResponse response = MovementControl(Commands.TestCommand,
                new RequestItem("LIGHT", (int)state));
            return "";
        }

        /// <summary>
        /// Returns MCU report including wheel encoders and IR obstacle avoidance.
        /// </summary>
        /// <returns></returns>
        public RovioMcuReport GetMCUReport()
        {
            RovioResponse dic = MovementControl(Commands.GetMCUReport);
            return RovioMcuReport.Parse(dic);
        }

        /// <summary>
        /// Deletes all paths in the robot’s Flash memory.
        /// </summary>
        /// <returns></returns>
        public string ClearAllPaths()
        {
            RovioResponse response = MovementControl(Commands.ClearAllPaths);
            return "";
        }


        /// <summary>
        /// Reports navigation state. 
        /// </summary>
        /// <remarks>Name changed from GetStatus (in API two fucntions with same name)</remarks>
        /// <returns></returns>
        public string GetNavStatus()
        {
            RovioResponse response = MovementControl(Commands.GetStatus);
            return "";
        }


        internal void SaveParameter(params FlashParameterItem[] parameters)
        {
            foreach (FlashParameterItem item in parameters)
            {
                this.SaveParameter(item.Key, item.Value);
            }
        }
        /// <summary>
        /// Stores parameter in the robot’s Flash memory.
        /// </summary>
        /// <param name="index">0 – 19</param>
        /// <param name="value">32bit signed integer</param>
        /// <returns></returns>
        internal void SaveParameter(FlashParameters parameterId, Int32 value)
        {
            RovioResponse response = MovementControl(Commands.SaveParameter,
                new RequestItem("index", (int)parameterId),
                new RequestItem("value", value));
        }


        /// <summary>
        /// Read parameter in the robot’s Flash memory.
        /// </summary>
        /// <param name="index">0 – 19</param>
        /// <returns></returns>
        public Int32 ReadParameter(long index)
        {
            RovioResponse response = MovementControl(Commands.ReadParameter,
                new RequestItem("index", index));
            return Int32.Parse(response["value"]);
        }

        internal Dictionary<FlashParameters, Int32> ReadAllParameters()
        {
            Dictionary<FlashParameters, Int32> dic = new Dictionary<FlashParameters, int>();
            RovioResponse response = MovementControl(Commands.ReadParameter);
            foreach(RovioResponseItem item in response)
            {
                if(!item.Key.StartsWith("v"))
                    continue;
                FlashParameters paraId = (FlashParameters)int.Parse(item.Key.Substring(1));
                int value = -1;
                if (string.IsNullOrEmpty(item.Value))
                    value = -1;
                else
                    value = Int32.Parse(item.Value);
                if (dic.ContainsKey(paraId))
                    dic[paraId] = value;
                else
                    dic.Add(paraId, value);
            }

            return dic;
        }

        /// <summary>
        /// Returns string version of libNS and NS sensor.
        /// </summary>
        /// <returns></returns>
        public string GetLibNSVersion()
        {
            RovioResponse response = MovementControl(Commands.GetLibNSVersion);
            return response["version"];
        }


        /// <summary>
        /// Emails current image or if in path recording mode sets an action.
        /// </summary>
        /// <param name="email">email address (hello@gmail.com)</param>
        /// <returns></returns>
        public string EmailImage(string email)
        {
            RovioResponse response = MovementControl(Commands.EmailImage,
                new RequestItem("email", email));
            return "";
        }

        /// <summary>
        /// Clears home location in the robot's Flash memory.
        /// </summary>
        /// <returns></returns>
        public string ResetHomeLocation()
        {
            RovioResponse response = MovementControl(Commands.ResetHomeLocation);
            return "";
        }

        /// <summary>
        /// Accepts manual driving commands.
        /// </summary>
        public ManualDriver ManualDriver
        {
            get { return this.driver; }
        }

        /// <summary>
        /// 控制命令枚举。
        /// </summary>
        internal enum Commands : int
        {
            /// <summary>
            /// Generates report of current status
            /// </summary>
            GetReport = 1,
            /// <summary>
            /// Start recording a path.
            /// </summary>
            StartRecoding = 2,
            /// <summary>
            /// Terminates recording a path
            /// </summary>
            AbortRecording = 3,
            /// <summary>
            /// Stop recording and store the path
            /// </summary>
            StopRecording = 4,
            /// <summary>
            /// Delete specific path
            /// </summary>
            Deletepath = 5,
            /// <summary>
            /// Return stored paths
            /// </summary>
            GetPathList = 6,
            /// <summary>
            /// Replay a stored path from closest point to the end
            /// </summary>
            PlayPathForward = 7,
            /// <summary>
            /// Replay a stored path from closest point to the beginning
            /// </summary>
            PlayPathBackward = 8,
            /// <summary>
            /// Stop playing a path
            /// </summary>
            StopPlaying = 9,
            /// <summary>
            /// Pause playing a path
            /// </summary>
            PausePlaying = 10,
            /// <summary>
            /// Rename the path name
            /// </summary>
            RenamePath = 11,
            /// <summary>
            /// Drive to home location without docking
            /// </summary>
            GoHome = 12,
            /// <summary>
            /// Drive to home location with docking
            /// </summary>
            GoHomeAndDock = 13,
            /// <summary>
            /// Update home location
            /// </summary>
            UpdateHomePosition = 14,
            /// <summary>
            /// Set homing, docking and driving parameters
            /// </summary>
            SetTuningParameters = 15,
            /// <summary>
            /// Return homing, docking and driving parameters
            /// </summary>
            GetTuningParameters = 16,
            /// <summary>
            /// Stop and reset to idle
            /// </summary>
            ResetNavStateMachine = 17,
            /// <summary>
            /// Accepts manual driving commands
            /// </summary>
            ManualDrive = 18,
            /// <summary>
            /// RESERVED  前灯开关。
            /// </summary>
            TestCommand = 19,
            /// <summary>
            /// Return MCU report
            /// </summary>
            GetMCUReport = 20,
            /// <summary>
            /// Delete all paths
            /// </summary>
            ClearAllPaths = 21,
            /// <summary>
            /// Return navigation status
            /// </summary>
            GetStatus = 22,
            /// <summary>
            /// Stores robot parameters
            /// </summary>
            SaveParameter = 23,
            /// <summary>
            /// Return robot parameters
            /// </summary>
            ReadParameter = 24,
            /// <summary>
            /// Return libNS and NS sensor versions
            /// </summary>
            GetLibNSVersion = 25,
            /// <summary>
            /// Email current image / set an action (in path recording mode)
            /// </summary>
            EmailImage = 26,
            /// <summary>
            /// Clear home location
            /// </summary>
            ResetHomeLocation = 27,
        }
    }
}
