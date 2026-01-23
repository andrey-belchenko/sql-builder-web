//using System;

//namespace sql.builder.TFS.AutoCheckIn.Commands
//{
//    internal class UndoCommand : ICommand
//    {
//        private AutoCheckInAnnouncer _announcer;
//        private TFSServer _server;
//        private string[] _projectPaths;

//        public UndoCommand(AutoCheckInAnnouncer announcer, TFSServer server, string[] projectPaths)
//        {
//            _announcer = announcer;
//            _server = server;
//            _projectPaths = projectPaths;
//        }

//        public void Execute()
//        {
//            string message = "Отменяем изменения в:"
//                             + Environment.NewLine
//                             + string.Join(Environment.NewLine, _projectPaths);

//            _announcer.Announce(AutoCheckInControllerStatus.Message, message);

//            bool result = _server.UndoChanges(_projectPaths);
//            if (result)
//            {
//                _announcer.Announce(AutoCheckInControllerStatus.Message, "Изменения успешно отменены");
//            }
//            else
//            {
//                throw new AutoCheckInTaskException("Не удалось отменить изменения");
//            }
//        }
//    }
//}