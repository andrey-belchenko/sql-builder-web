using System;

namespace sql.builder.TFS.AutoCheckIn
{
    internal class AutoCheckInAnnouncer
    {
        internal event EventHandler<AutoCheckInControllerEventArgs> SomethingHappened = delegate { };

        internal void Announce(AutoCheckInControllerStatus status, string message)
        {
            SomethingHappened(this, new AutoCheckInControllerEventArgs(status, message, DateTime.Now));
        }

        internal bool HasChanges { get; set; }
        internal bool HasConflicts { get; set; }

        internal void Clear()
        {
            HasChanges = false;
            HasConflicts = false;
        }
    }
}