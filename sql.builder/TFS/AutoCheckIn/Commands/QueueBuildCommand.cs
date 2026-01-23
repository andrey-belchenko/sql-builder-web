//using System;

//namespace sql.builder.TFS.AutoCheckIn.Commands
//{
//    internal class QueueBuildCommand : ICommand
//    {
//        private AutoCheckInAnnouncer _announcer;
//        private TFSServer _server;
//        private string _buildName;

//        public QueueBuildCommand(AutoCheckInAnnouncer announcer, TFSServer server, string buildName)
//        {
//            _announcer = announcer;
//            _server = server;
//            _buildName = buildName;
//        }

//        public void Execute()
//        {
//            _announcer.Announce(AutoCheckInControllerStatus.Message, string.Format("Запускаем билд {0}...", _buildName));

//            bool result = _server.QueueBuild(_buildName);
//            if (result)
//            {
//                _announcer.Announce(AutoCheckInControllerStatus.Message, "Билд успешно запущен");
//            }
//            else
//            {
//                throw new AutoCheckInTaskException("Не удалось запустить билд");
//            }
//        }
//    }
//}