//using System;

//namespace sql.builder.TFS.AutoCheckIn.Commands
//{
//    internal class CheckInCommand : ICommand
//    {
//        const string AUTO_CHECKIN_MARK = "[auto]";

//        private AutoCheckInAnnouncer _announcer;
//        private TFSServer _server;
//        private string[] _projectPaths;
//        private string _comment;

//        public CheckInCommand(AutoCheckInAnnouncer announcer, TFSServer server, string[] projectPaths, string comment)
//        {
//            _announcer = announcer;
//            _server = server;
//            _projectPaths = projectPaths;
//            _comment = comment;
//        }

//        public void Execute()
//        {
//            string message = "Возврат изменений на сервер для:"
//                             + Environment.NewLine
//                             + string.Join(Environment.NewLine, _projectPaths);

//            _announcer.Announce(AutoCheckInControllerStatus.Message, message);

//            string comment = string.IsNullOrEmpty(_comment) ? AUTO_CHECKIN_MARK : (_comment + " " + AUTO_CHECKIN_MARK);

//            TFSCheckInResult result = _server.CheckIn(_projectPaths, null, comment);
//            if (result.Success)
//            {
//                if (result.GatedBuildName != null)
//                {
//                    _announcer.Announce(AutoCheckInControllerStatus.Message,
//                        string.Format("Изменения поставлены в очередь. Запущен проверочный билд {0}", result.GatedBuildName));
//                }
//                else
//                {
//                    _announcer.Announce(AutoCheckInControllerStatus.Message,
//                        "Изменения успешно возвращены");
//                }
//            }
//            else
//            {
//                throw new AutoCheckInTaskException("Не удалось вернуть изменения:" + Environment.NewLine + result.ErrorMessage);
//            }
//        }
//    }
//}