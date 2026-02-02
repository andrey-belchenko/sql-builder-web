using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.XmlHelpers
{
    public static class Logger
    {
        public static decimal ReportStart(string repname, XElement report_params)
        {
            return db.InsertReportLog(repname, report_params != null ? report_params.ToString() : @"<params\>");
        }

        public static void ReportFinish(decimal kod_log)
        {
            // чтобы триггер сработал
            db.UpdateReportLog(kod_log, null, null);
        }

        public static void ReportError(decimal kod_log, string error_text, string stack_text)
        {
            db.UpdateReportLog(kod_log,
                error_text.Length > 999 ? error_text.Substring(0, 999) : error_text,
                stack_text.Length > 999 ? stack_text.Substring(0, 999) : stack_text);
        }

        public static event Action<Tuple<DateTime, string>> LogEvent;
        private static List<Tuple<DateTime, string>> _session_log;
        public static bool IsAcive { get; private set; }
        public static void BeginSession()
        {
            IsAcive = true;
            _session_log = new List<Tuple<DateTime, string>>();
        }

        public static void Log(string text)
        {
            var data = new Tuple<DateTime, string>(DateTime.Now, text);
            _session_log.Add(data);

            if (LogEvent != null) LogEvent(data);
        }

        public static Tuple<DateTime, string>[] GetSessionLog()
        {
            return _session_log.ToArray();
        }

        public static void EndSession()
        {
            IsAcive = false;
            _session_log = null;
            LogEvent = null;
        }
    }
}
