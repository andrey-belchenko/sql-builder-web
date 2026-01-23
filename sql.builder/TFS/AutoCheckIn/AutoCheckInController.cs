//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using sql.builder.TFS.AutoCheckIn.Commands;
//using sql.builder.TFS.AutoCheckIn.Config;
//using sql.builder.XmlHelpers;

//namespace sql.builder.TFS.AutoCheckIn
//{
//    internal class AutoCheckInController
//    {
//        private string comment;
//        private AutoCheckInAnnouncer announcer;
//        private AutoCheckInFolderSetting main_folders_settings;
//        private AutoCheckInFolderSetting[] branch_folders_settings;
//        private AutoCheckInProjectSetting[] projects_settings;
//        private string[] custom_always_local_paths;  // костыль - локальные пути!
//        private string[] custom_always_server_paths;
//        private TFSServer server;
//        private Task _task;
//        private CancellationTokenSource _cancellationSource;
//        //
//        internal string Comment { get { return this.comment; } set { this.comment = value; } }
//        internal AutoCheckInAnnouncer Announcer { get { return this.announcer; } }
//        internal AutoCheckInFolderSetting MainFoldersSettings { get { return this.main_folders_settings; } }
//        internal AutoCheckInFolderSetting[] BranchFoldersSettings { get { return this.branch_folders_settings; } }
//        internal AutoCheckInProjectSetting[] ProjectsSettings { get { return this.projects_settings; } }
//        internal void AddCustomAlwaysLockalPaths(IEnumerable<string> paths)
//        {
//            this.custom_always_local_paths = this.custom_always_local_paths.Concat(paths).Distinct().ToArray();
//        }
//        internal void AddCustomAlwaysServerPaths(IEnumerable<string> paths)
//        {
//            this.custom_always_server_paths = this.custom_always_server_paths.Concat(paths).Distinct().ToArray();
//        }
//        internal AutoCheckInController(TFSServer server)
//        {
//            this.server = server;
//            var config = ConfigurationManager.GetSection("tfsAutoCheckInConfig") as TFSAutoCheckInConfig;
//            int index;
//            FoldersCollection folders = config.Folders;
//            // первая считается главной веткой
//            this.main_folders_settings = new AutoCheckInFolderSetting(folders[0]);
//            // остальные - ветками версий
//            this.branch_folders_settings = new AutoCheckInFolderSetting[folders.Count - 1];
//            for (index = 0; index < folders.Count - 1; index++) {
//                this.branch_folders_settings[index] = new AutoCheckInFolderSetting(folders[index + 1]);
//            }
//            //
//            ProjectsCollection projects = config.Projects;
//            this.projects_settings = new AutoCheckInProjectSetting[projects.Count];
//            for (index = 0; index < projects.Count; index++) {
//                this.projects_settings[index] = new AutoCheckInProjectSetting(projects[index]);
//            }
//            //
//            this.announcer = new AutoCheckInAnnouncer();
//            this.custom_always_local_paths = Array.Empty<string>();
//            this.custom_always_server_paths = Array.Empty<string>();
//            this.LoadState();
//        }
//        internal void InitializeAsync()
//        {
//            Reset();
//            QueueCommand(Initialize);
//        }
//        internal bool StartAsync()
//        {
//            if (_task != null && (_task.Status == TaskStatus.Running || _task.Status == TaskStatus.WaitingToRun))
//            {
//                return false;
//            }

//            Reset();

//            string mainFolderPath = this.main_folders_settings.Config.Name;
//            var projectsConfig = this.projects_settings.Where(ps => ps.Selected).Select(ps => ps.Config).ToArray();
//            string[] mainProjectPaths = projectsConfig.Select(prj => mainFolderPath + prj.Name);

//            if (mainProjectPaths.Length == 0) return true;

//            // нужно забрать изменения
//            if (this.main_folders_settings.GetChanges)
//            {
//                string[] resolveAsServerPaths = projectsConfig
//                    .SelectMany(prj => prj.TakeServerPaths.Cast<PathElement>().Select(p => mainFolderPath + prj.Name + "/" + p.Name))
//                    .ToArray();
//                string[] resolveAsLocalPaths = projectsConfig
//                    .SelectMany(prj => prj.TakeLocalPaths.Cast<PathElement>().Select(p => mainFolderPath + prj.Name + "/" + p.Name))
//                    .ToArray();

//                var options = TFSResolveConflictsOptions.Default;
//                if (resolveAsServerPaths.Length != 0 || resolveAsLocalPaths.Length != 0 || custom_always_local_paths.Length != 0 || custom_always_server_paths.Length != 0)
//                {
//                    options.Mode = TFSResolveConflictsMode.AnalizeCustomPaths;
//                    // пути к файлам на сервере
//                    options.ServerPathsTakeLocalVersion.AddRange(resolveAsLocalPaths);
//                    options.ServerPathsTakeServerVersion.AddRange(resolveAsServerPaths);
//                    // локальные пути к файлам
//                    options.LocalPathsTakeLocalVersion.AddRange(custom_always_local_paths);
//                    options.LocalPathsTakeServerVersion.AddRange(custom_always_server_paths);
//                }
//                QueueCommand(new GetCommand(this.announcer, server, mainProjectPaths));
//                QueueCommand(new ResolveConflictsCommand(this.announcer, server, mainProjectPaths, options));
//            }

//            // нужно перестроить схему
//            if (this.main_folders_settings.RebuildScheme > 0)
//            {
//                QueueCommand(new RebuildSchemeCommand(this.announcer, this.main_folders_settings.RebuildScheme));
//            }

//            // нужно вернуть изменения
//            if (this.main_folders_settings.CheckIn)
//            {
//                QueueCommand(new CheckInCommand(this.announcer, server, mainProjectPaths, Comment));
//            }

//            // построить билд
//            if (this.main_folders_settings.QueueBuild)
//            {
//                string buildName = this.main_folders_settings.Config.BuildName;
//                QueueCommand(new QueueBuildCommand(this.announcer, server, buildName));
//            }

//            foreach (var branchFolderSetting in this.branch_folders_settings)
//            {
//                string[] forbiddenProjectNames = branchFolderSetting.Config.ForbiddenProjects.Cast<ProjectElement>().Select(pe => pe.Name).ToArray();

//                string branchFolderPath = branchFolderSetting.Config.Name;
//                string[] branchProjectPaths = projectsConfig
//                    .Where(prj => !forbiddenProjectNames.Contains(prj.Name))
//                    .Select(prj => branchFolderPath + prj.Name)
//                    .ToArray();

//                Tuple<string, string>[] mergePaths = projectsConfig
//                    .Where(prj => !forbiddenProjectNames.Contains(prj.Name))
//                    .Select(prj => new Tuple<string, string>(mainFolderPath + prj.Name, branchFolderPath + prj.Name))
//                    .ToArray();

//                bool gatedCheckIn = branchFolderSetting.Config.GatedCheckIn;
//                string buildName = branchFolderSetting.Config.BuildName;

//                // при протаскивании AcceptTheirs значит взять изменения из ветки main
//                var options = TFSResolveConflictsOptions.Default;
//                options.Mode = TFSResolveConflictsMode.TakeServerVersionForAll;

//                // нужно протянуть изменения
//                if (branchFolderSetting.GetChanges)
//                {
//                    if (forbiddenProjectNames.Length != 0) {
//                        string text = string.Format("Следующие проекты не будут протянуты в ветвь {0}, т.к. стоит запрет в настройках:\r\n{1}", 
//                            branchFolderPath, string.Join("\r\n", forbiddenProjectNames));
//                        QueueCommand(() => this.announcer.Announce(AutoCheckInControllerStatus.Message, text));
                       
//                    }
//                    QueueCommand(new UndoCommand(this.announcer, server, branchProjectPaths));
//                    QueueCommand(new GetCommand(this.announcer, server, branchProjectPaths));

//                    QueueCommand(new ResolveConflictsCommand(this.announcer, server, branchProjectPaths, options));
//                    QueueCommand(new MergeCommand(this.announcer, server, mergePaths));
//                }

//                if (branchFolderSetting.CheckIn)
//                {
//                    QueueCommand(new ResolveConflictsCommand(this.announcer, server, branchProjectPaths, options));
//                    QueueCommand(new CheckInCommand(this.announcer, server, branchProjectPaths, Comment));
//                    // нужно чтобы снять блокировки с бинарных файлов, иначе gated build не построится
//                    QueueCommand(new UndoCommand(this.announcer, server, branchProjectPaths));
//                }

//                // построить билд
//                if (branchFolderSetting.QueueBuild && !gatedCheckIn)
//                {
//                    QueueCommand(new QueueBuildCommand(this.announcer, server, buildName));
//                }
//            }
            
//            if(_task != null)
//            {
//                // при отмене пользователем
//                _task.ContinueWith(t => OnCanceled(), TaskContinuationOptions.OnlyOnCanceled);
//                // при успешном завершении всех задач
//                _task.ContinueWith(t => OnSuccess(), TaskContinuationOptions.NotOnFaulted);      
//            }
//            else
//            {
//                // не было задач - сразу сигнализируем об успехе
//                OnSuccess();
//            }

//            return true;
//        }
//        internal void Cancel()
//        {
//            if(_cancellationSource != null)
//            {
//                _cancellationSource.Cancel();
//            }
//        }

//        internal void ClearFoldersSettings()
//        {
//            this.main_folders_settings.Clear();
//            //var folders = MainFoldersSettings.Concat(BranchFoldersSettings);
//            //foreach (var folder in BranchFoldersSettings) {
//            for (int index = 0; index < this.branch_folders_settings.Length; index++) {
//                this.branch_folders_settings[index].Clear();
//                //folder.GetChanges = false;
//                //folder.RebuildScheme = 0;
//                //folder.CheckIn = false;
//                //folder.QueueBuild = false;
//            }
//        }

//        private void Reset()
//        {
//            _task = null;
//            _cancellationSource = new CancellationTokenSource();
//            this.announcer.Clear();
//        }

//        private void QueueCommand(ICommand command)
//        {
//            QueueCommand(command.Execute);  
//        }
//        private void QueueCommand(Action commandFunc)
//        {
//            var token = _cancellationSource.Token;

//            if (_task == null)
//            {
//                _task = Task.Factory.StartNew(commandFunc, token);
//            }
//            else
//            {
//                // если предыдущая задача закончилась без ошибки и не отменена
//                _task = _task.ContinueWith(t => commandFunc(), token, TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
//            }

//            // при ошибке
//            _task.ContinueWith(OnException, TaskContinuationOptions.OnlyOnFaulted);
//        }

//        private void Initialize()
//        {
//            this.announcer.Announce(AutoCheckInControllerStatus.Message, string.Format("Подключение к {0} ...", TFSServer.URI));
//            try
//            {
//                server.Connect();
//            }
//            catch
//            {
//                throw new AutoCheckInTaskException("Не удалось подключиться");
//            }

//            this.announcer.Announce(AutoCheckInControllerStatus.Message, "Подключение установлено");
//            this.announcer.Announce(AutoCheckInControllerStatus.Completed, null);
//        }

//        private void OnException(Task prevTask)
//        {
//            this.announcer.Announce(AutoCheckInControllerStatus.Error, prevTask.Exception.InnerException.Message);
//            this.announcer.Announce(AutoCheckInControllerStatus.Completed, null);
//        }
//        private void OnSuccess()
//        {
//            this.announcer.Announce(AutoCheckInControllerStatus.Completed, null);
//        }
//        private void OnCanceled()
//        {
//            this.announcer.Announce(AutoCheckInControllerStatus.Message, "Отменено пользователем");
//            this.announcer.Announce(AutoCheckInControllerStatus.Completed, null);
//        }

//        internal void LoadState()
//        {
//            Comment = SettingsHelper.TFSComment;
//        }

//        internal void SaveState()
//        {
//            SettingsHelper.TFSComment = Comment;
//        }
//    }
//}