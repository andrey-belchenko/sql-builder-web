using System;
using sql.builder.DataApi;

namespace sql.builder.TFS.AutoCheckIn.Commands
{
    internal class RebuildSchemeCommand : ICommand
    {
        private AutoCheckInAnnouncer _announcer;
        private int _rebuildScheme;

        public RebuildSchemeCommand(AutoCheckInAnnouncer announcer, int rebuildScheme)
        {
            _announcer = announcer;
            _rebuildScheme = rebuildScheme;
        }

        public void Execute()
        {
            // не было загружено файлов, а перебилдить надо только если файлы были
            // + если были изменения в схеме
            bool hasLocalChanges = XmlReports.IsAnySchemeChanged();
            if (_rebuildScheme == 1 && !_announcer.HasChanges && !hasLocalChanges)
            {
                return;
            }

            if (!_announcer.HasChanges && hasLocalChanges) _announcer.Announce(AutoCheckInControllerStatus.Message, "Имеются локальные изменения в схеме");
            _announcer.Announce(AutoCheckInControllerStatus.Message, "Перекомпиляция схемы...");

            try
            {
                if (_rebuildScheme == 2 || _announcer.HasChanges)
                {
                    XmlReports.Init(true);    
                }
                else
                {
                    XmlReports.Init(false);  
                }
                        
                
                VCashUtils.ClearCash();
            }
            catch (Exception ex)
            {
                throw new AutoCheckInTaskException("Не удалось перекомпилировать:" + Environment.NewLine + ex.Message);
            }

            _announcer.Announce(AutoCheckInControllerStatus.Message, "Перекомпиляция прошла успешно");
        }
    }
}