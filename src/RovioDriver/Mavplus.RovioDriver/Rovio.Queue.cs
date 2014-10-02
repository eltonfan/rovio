using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Mavplus.RovioDriver.Commands;
using System.Diagnostics;
using System.Threading;
using Mavplus.RovioDriver.Events;

namespace Mavplus.RovioDriver
{
    partial class Rovio
    {
        const int commandRefreshTimeout = 50;

        // Event handlers
        public event ErrorEventHandler Error;

        readonly ConcurrentQueue<Command> queueCommands = new ConcurrentQueue<Command>();

        private const int workerThreadCloseTimeout = 10000;

        // Threading
        protected Thread workerThread;
        protected bool workerThreadEnded = false;

        protected void StartWorkerThread()
        {
            workerThreadEnded = false;

            workerThread = new Thread(new ThreadStart(ProcessWorkerThreadInternally));
            workerThread.Name = this.GetType().ToString() + "_WorkerThread";
            workerThread.Start();
        }

        protected virtual void ProcessWorkerThreadInternally()
        {
            try
            {
                ProcessWorkerThread();
            }
            catch (Exception e)
            {
                ProcessThreadedException(e);
            }
        }

        protected virtual void ProcessWorkerThread()
        {
            Stopwatch stopwatch = new Stopwatch();

            //if (!IsInitialized())
            //    Initialize();

            //SendQueuedCommand(new SetControlModeCommand(DroneControlMode.LogControlMode));
            //SetDefaultCamera();

            do
            {
                stopwatch.Restart();

                //SendQueuedCommand(new WatchDogCommand());

                Command command;
                if (!queueCommands.TryDequeue(out command))
                {
                    Thread.Sleep(50);
                    continue;
                }
                ProcessCommand(command);


                stopwatch.Stop();

                if (commandRefreshTimeout > stopwatch.ElapsedMilliseconds)
                    Thread.Sleep((int)(commandRefreshTimeout - stopwatch.ElapsedMilliseconds));
            }
            while (!workerThreadEnded);
        }

        protected void WaitForWorkerThreadToEnd()
        {
            workerThread.Join(workerThreadCloseTimeout);
            workerThread.Abort();
            workerThread = null;
        }

        private void StopWorkerThread()
        {
            workerThreadEnded = true;
            WaitForWorkerThreadToEnd();
        }

        private void ProcessThreadedException(Exception e)
        {
            if (Error != null)
                Error.Invoke(this, new NetworkWorkerErrorEventArgs(e));

            workerThreadEnded = true;
            //Connected = false;
        }

        public void SendQueuedCommand(Command command)
        {
            //command.SequenceNumber = GetSequenceNumberForCommand();
            //commandsToSend.Add(command.CreateCommand(FirmwareVersion));

            //if (command is SetConfigurationCommand)
            //{
            //    SetControlModeCommand controlModeCommand = new SetControlModeCommand(DroneControlMode.LogControlMode);
            //    controlModeCommand.SequenceNumber = GetSequenceNumberForCommand();
            //    commandsToSend.Add(controlModeCommand.CreateCommand(FirmwareVersion));
            //}

            if (command is FlightMoveCommand)
            {
                FlightMoveCommand moveCommand = (FlightMoveCommand)command;
                this.SetFlightParameters(moveCommand.Roll, moveCommand.Pitch, moveCommand.Yaw, moveCommand.Gaz);
                return;
            }

            queueCommands.Enqueue(command);

        }

        private void SendUnqueuedCommand(Command command)
        {
            //command.SequenceNumber = GetSequenceNumberForCommand();
            //SendMessage(command.CreateCommand(FirmwareVersion));
        }

        void ProcessCommand(Command command)
        {
            if (command is FlightMoveCommand)
            {
                FlightMoveCommand moveCommand = (FlightMoveCommand)command;

            }
        }
    }
}
