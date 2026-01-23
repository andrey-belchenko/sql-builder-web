//using System;

//namespace sql.builder.TFS.AutoCheckIn.Commands
//{
//    internal class GetCommand : ICommand
//    {
//        private AutoCheckInAnnouncer _announcer;
//        private TFSServer _server;
//        private string[] _projectPaths;

//        public GetCommand(AutoCheckInAnnouncer announcer, TFSServer server, string[] projectPaths)
//        {
//            _announcer = announcer;
//            _server = server;
//            _projectPaths = projectPaths;
//        }

//        public void Execute()
//        {
//            _announcer.HasConflicts = false;
//            _announcer.HasChanges = false;

//            string message = "Загрузка изменений с сервера для:" 
//                + Environment.NewLine 
//                + string.Join(Environment.NewLine, _projectPaths);

//            _announcer.Announce(AutoCheckInControllerStatus.Message, message);

//            TFSGetResult getResult = _server.Get(_projectPaths);

//            if (getResult.Success)
//            {
//                _announcer.HasConflicts = (getResult.NumConflicts > 0);
//                if ((getResult.NumConflicts + getResult.NumUpdated + getResult.NumFailures) == 0)
//                {
//                    _announcer.HasChanges = false;
//                    _announcer.Announce(AutoCheckInControllerStatus.Message, "Изменения на сервере отсутствуют");
//                }
//                else if (getResult.NumFailures > 0)
//                {
//                    message = string.Format("Не удалось обновить: {2}, Обновлено файлов: {0}, Обнаружено конфликтов: {1}", 
//                        getResult.NumUpdated, getResult.NumConflicts, getResult.NumFailures);

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
//                throw new AutoCheckInTaskException(getResult.ErrorMessage);
//            }
//        }
//    }
//}