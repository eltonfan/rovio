using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    /// <summary>
    /// Provides a way to control the ARDrone.
    /// </summary>
    public partial interface IRovio
    {
        /// <summary>
        /// Starts the engines of the ARDrone. The ARDrone will take of and start hovering at a predetermined height.
        /// </summary>
        void StartEngines();
        /// <summary>
        /// Stops the ARDrone engines, the ARDrone will land smoothly.
        /// </summary>
        void StopEngines();
        /// <summary>
        /// Starts issuing an emergency reset command to the ARDrone. The engines will stop immediately and ARDrone stops hovering. In most cases this will lead to a crash.
        /// </summary>
        void StartReset();
        /// <summary>
        /// Stops issuing an emergency reset command to the ARDrone. 
        /// </summary>
        void StopReset();
        /// <summary>
        /// Starts the recording of video images.
        /// </summary>
        bool StartRecordVideo();
        /// <summary>
        /// Stops the recording of video images.
        /// </summary>
        /// <returns><c>true</c> if path where video files are stored exists; otherwise, <c>false</c>.</returns>
        void StopRecordVideo();
        /// <summary>
        /// Saves the current image as a snapshot.
        /// </summary>     
        /// <returns><c>true</c> if path where pictures are stored exists; otherwise, <c>false</c>.</returns>
        bool TakePicture();
        /// <summary>
        /// Sets the flight parameters. This allows to pilot the ARDrone.
        /// </summary>
        /// <param name="roll">The roll parameter (Tilt Left/Right - Phi angle).</param>
        /// <param name="pitch">The pitch parameter (Tilt Front/Back - Theta angle)</param>
        /// <param name="height">The height parameter. (Move Up/Down)</param>
        /// <param name="yaw">The yaw parameter. (Rotate Left/Right - Psi angle)</param>
        /// <remarks>All parameters have a value between -1 and 1.</remarks>
        void SetFlightParameters(float roll, float pitch, float yaw, float gaz = 0.0F);
        /// <summary>
        /// Makes the ARDrone animate its LED's.
        /// </summary>
        /// <param name="id">The id of the animation to play.</param>
        /// <param name="frequency">The blink frequency of the animation in times per second (Hz).</param>
        /// <param name="duration">The duration of the animation in seconds.</param>
        void PlayLedAnimation(int id, int frequency, int duration);
        /// <summary>
        /// Displays the next video channel.
        /// </summary>
        void DisplayNextVideoChannel();
        /// <summary>
        /// Switch to the selected video channel.
        /// </summary>
        void SwitchVideoChannel(int videoChannel);
        /// <summary>
        /// Sets the flat trim. Has to be called before each new flight.
        /// </summary>
        void SetTakePicture();
        /// <summary>
        /// Issues a command to the ARDrone to start detecting predefined coloured patterns (orrange|yellow|orange, orange|green|orange and orange|blue|orange).
        /// </summary>
        void StartVisionDetect();
        /// <summary>
        /// Issues a command to the ARDrone to stop detecting predefined coloured patterns. 
        /// </summary>
        void StopVisionDetect();
    }
}
