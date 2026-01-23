//using System;
//using System.Linq;

//namespace sql.builder.TFS.AutoCheckIn.Commands
//{
//    internal class MergeCommand : ICommand
//    {
//        private AutoCheckInAnnouncer _announcer;
//        private TFSServer _server;
//        private Tuple<string, string>[] _mergePaths;

//        public MergeCommand(AutoCheckInAnnouncer announcer, TFSServer server, Tuple<string,string>[] mergePaths)
//        {
//            _announcer = announcer;
//            _server = server;
//            _mergePaths = mergePaths;
//        }

//        public void Execute()
//        {
//            _announcer.HasConflicts = false;
//            _announcer.HasChanges = false;

//            string message = "Протягиваем изменения в:"
//                             + Environment.NewLine
//                             + string.Join(Environment.NewLine, _mergePaths.Select(t => t.Item2));

//            _announcer.Announce(AutoCheckInControllerStatus.Message, message);

//            TFSGetResult getResult = _server.Merge(_mergePaths);
//            if (getResult.Success)
//            {
//                _announcer.HasConflicts = (getResult.NumConflicts > 0);

//                if ((getResult.NumConflicts + getResult.NumUpdated + getResult.NumFailures) == 0)
//                {
//                    _announcer.HasChanges = false;
//                    _announcer.Announce(AutoCheckInControllerStatus.Message, "Нет изменений для протягивания");
//                }
//                else if (getResult.NumFailures > 0)
//                {
//                    message = string.Format("Не удалось обновить: {2}, Обновлено файлов: {0}, Обнаружено конфликтов: {1}\r\n{3}",
//                        getResult.NumUpdated, getResult.NumConflicts, getResult.NumFailures, getResult.FailuresMessage.TrimEnd());

//                    throw new AutoCheckInTaskException(message);
//                }
//                else
//                {
//                    _announcer.HasChanges = true;
//                    _announcer.Announce(AutoCheckInControllerStatus.Message, string.Format("Успешно. Обновлено файлов: {0}, Обнаружено конфликтов: {1}",
//                        getResult.NumUpdated, getResult.NumConflicts));
//                }
//            }
//            else
//            {
//                throw new AutoCheckInTaskException("Не удалось протянуть изменения:" + Environment.NewLine + getResult.ErrorMessage);
//            }
//        }
//    }
//}