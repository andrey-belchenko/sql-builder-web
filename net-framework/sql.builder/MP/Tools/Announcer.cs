using System;

namespace sql.builder.MP.Tools
{
    public class Announcer
    {
        public event EventHandler<AnnouncerEventArgs> SomethingHappened = delegate { };

        public void Announce(string message, MessageStatus status)
        {
            SomethingHappened(this, new AnnouncerEventArgs(message, status, DateTime.Now));
        }
    }

    public enum MessageStatus
    {
        Normal,
        Error
    }

    public class AnnouncerEventArgs : EventArgs
    {
        internal MessageStatus Status { get; private set; }
        internal string Message { get; private set; }
        internal DateTime Time { get; private set; }

        public AnnouncerEventArgs(string message, MessageStatus status, DateTime time)
        {
            Message = message;
            Status = status;
            Time = time;
        }
    }
}