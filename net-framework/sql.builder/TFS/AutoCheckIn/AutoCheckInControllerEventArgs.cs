using System;

namespace sql.builder.TFS.AutoCheckIn
{
    internal class AutoCheckInControllerEventArgs : EventArgs
    {
        internal AutoCheckInControllerStatus Status { get; private set; }
        internal string Message { get; private set; }
        internal DateTime Time { get; private set; }

        public AutoCheckInControllerEventArgs(AutoCheckInControllerStatus status, string message, DateTime time)
        {
            Status = status;
            Message = message;
            Time = time;
        }
    }
}