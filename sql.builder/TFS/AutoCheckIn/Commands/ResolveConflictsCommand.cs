//using System;
//using System.Linq;

//namespace sql.builder.TFS.AutoCheckIn.Commands
//{
//    internal class ResolveConflictsCommand : ICommand
//    {
//        private AutoCheckInAnnouncer _announcer;
//        private TFSServer _server;
//        private TFSResolveConflictsOptions _options;
//        private string[] _projectPaths;
//        private string[] _resolveAsServerPaths;
//        private string[] _resolveAsLocalPaths;
//        private bool? _alwaysLocal;

//        public ResolveConflictsCommand(AutoCheckInAnnouncer announcer, TFSServer server, string[] projectPaths, TFSResolveConflictsOptions options = null)
//        {
//            _announcer = announcer;
//            _server = server;
//            _projectPaths = projectPaths;
//            _options = options;
//        }

//        public void Execute()
//        {
//            // не было конфликтов
//            if (!_announcer.HasConflicts)
//            {
//                return;
//            }

//            string message = "Разрешение конфликтов для:"
//                             + Environment.NewLine
//                             + string.Join(Environment.NewLine, _projectPaths);
//            _announcer.Announce(AutoCheckInControllerStatus.Message, message);

//            TFSResolveResult resolveResult = _server.ResolveConflicts(_projectPaths, _options);

//            if (resolveResult.Success)
//            {
//                if (resolveResult.UnresolvedFiles.Length == 0)
//                {
//                    _announcer.Announce(AutoCheckInControllerStatus.Message, string.Format("Успешно. Разрешено конфликтов: {0}",
//                        resolveResult.NumResolvedConflicts));
//                }
//                else
//                {
//                    message = "Требуется ручное разрешение конфликтов для файлов:" 
//                              + Environment.NewLine
//                              + string.Join(Environment.NewLine, resolveResult.UnresolvedFiles.Select(f => f.ServerPath));
//                    throw new AutoCheckInTaskException(message);
//                }
//            }
//            else
//            {
//                throw new AutoCheckInTaskException(resolveResult.ErrorMessage);
//            }
//        }
//    }
//}