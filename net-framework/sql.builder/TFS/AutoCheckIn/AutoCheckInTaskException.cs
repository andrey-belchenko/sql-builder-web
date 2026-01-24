using System;

namespace sql.builder.TFS.AutoCheckIn
{
    internal class AutoCheckInTaskException: ApplicationException
    {
        internal AutoCheckInTaskException(string message): base(message)
        {
            
        }
    }
}