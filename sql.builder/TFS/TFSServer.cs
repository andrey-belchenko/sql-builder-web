//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.TeamFoundation.Build.Client;
//using Microsoft.TeamFoundation.Client;
//using Microsoft.TeamFoundation.VersionControl.Client;
//using Microsoft.TeamFoundation.WorkItemTracking.Client;

//namespace sql.builder.TFS
//{
//    /// <summary>
//    /// Класс для работы с TFS через API
//    /// </summary>
//    internal class TFSServer : IDisposable
//    {
//        internal const string URI = @"http://tfs.infoenergo.loc:8080/tfs/infoenergo/";
//        private TfsTeamProjectCollection _tfs;
//        private WorkItemStore _wistore;
//        private Workspace _ws;
//        private bool _connected;
//        public TFSServer(bool connect = true)
//        {
//            if (connect) Connect();
//        }
//        /// <summary>
//        /// Подключение к TFS. Этот метод необходимо вызвать первым
//        /// </summary>
//        public void Connect()
//        {
//            if (this._connected) return;
//            Workstation workstation = Workstation.Current;
//            WorkspaceInfo[] wi = workstation.GetAllLocalWorkspaceInfo();
//            string workspace_name, workspace_owner;
//            Uri server_uri;
//            if (wi != null && wi.Length > 0) {
//                workspace_name = wi[0].Name;
//                workspace_owner = wi[0].OwnerName;
//                server_uri = wi[0].ServerUri;
//            } else {
//                workspace_name = Environment.MachineName;
//                workspace_owner = Environment.UserName;
//                server_uri = new Uri(URI);
//            }
            
//            this._tfs = new TfsTeamProjectCollection(server_uri);
//            VersionControlServer vcs = _tfs.GetService<VersionControlServer>();

//            try
//            {
//                workstation.EnsureUpdateWorkspaceInfoCache(vcs, workspace_owner);
//            }
//            catch
//            {
//                // TODO: Костыль, вынести в параметры
//                workspace_owner = "infoenergo\\abelchenko";
//                workstation.EnsureUpdateWorkspaceInfoCache(vcs, workspace_owner);
//            }
//            this._ws = vcs.GetWorkspace(workspace_name, workspace_owner);

//            try
//            {
//                // Бельченко 2025-01-26
//                // не работаает на новой машине, не хватает ,библиотек
//                // по факту вроде не используется
//                this._wistore = new WorkItemStore(this._tfs);
//            }
//            catch
//            {

//            }
           
//            this._connected = true;
//        }
//        /// <summary>
//        /// Загрузка последней версии файлов
//        /// </summary>
//        /// <param name="projectPaths">Пути к папкам на сервере, для которых будет загружена последняя версия рекурсивно</param>
//        public TFSGetResult Get(string[] projectPaths)
//        {
//            // забираем все что забирается
//            GetRequest[] getRequests = new GetRequest[projectPaths.Length];
//            for (int index = 0; index < projectPaths.Length; index++) {
//                getRequests[index] = new GetRequest(new ItemSpec(projectPaths[index], RecursionType.Full), VersionSpec.Latest);
//            }
//            TFSGetResult result = null;
//            try {
//                GetStatus getResult = this._ws.Get(getRequests, GetOptions.None);
//                result = new TFSGetResult();
//                result.Success = true;
//                result.NumFailures = getResult.NumFailures;
//                result.NumUpdated = getResult.NumUpdated;
//                result.NumConflicts = getResult.NumConflicts;
//            } catch (Exception ex) {
//                result = new TFSGetResult();
//                result.Success = false;
//                result.ErrorMessage = ex.Message;
//            }
//            return result;
//        }
//        /// <summary>
//        /// Разрешение конфликтов для указанных папок
//        /// </summary>
//        /// <param name="projectPaths">Пути к папкам на сервере, для которых будет загружена последняя версия рекурсивно</param>
//        public TFSResolveResult ResolveConflicts(string[] projectPaths, TFSResolveConflictsOptions options = null)
//        {
//            options = options ?? TFSResolveConflictsOptions.Default;

//            // собираем информацию о файлах, для которых не удалось разрешить конфликты
//            var unresolvedFilesList = new List<TFSFileInfo>();
            
//            // разбиваем на кучки, так как можно резолвить конфликты только одного типа за раз
//            var resolveAsServerConflicts = new List<Conflict>();
//            var resolveAsLocalConflicts = new List<Conflict>();
//            var unresolvedConflicts = new List<Conflict>();

//            TFSResolveResult result = null;
//            // получаем список конфликтов
//            try
//            {
//                Conflict[] conflicts = _ws.QueryConflicts(projectPaths, true);

//                foreach (var conflict in conflicts)
//                {
//                    if ((options.Mode == TFSResolveConflictsMode.TakeServerVersionForAll) 
//                        || (options.Mode == TFSResolveConflictsMode.AnalizeCustomPaths 
//                            && (options.ServerPathsTakeServerVersion.Any(s => (conflict.ServerPath).StartsWith(s)) 
//                                || (options.LocalPathsTakeServerVersion.Any(s => (conflict.LocalPath).StartsWith(s))))))
//                    {
//                        // забираем файл с сервера, затирая локальный
//                        conflict.Resolution = Resolution.AcceptTheirs;
//                        resolveAsServerConflicts.Add(conflict);
//                    }
//                    else if ((options.Mode == TFSResolveConflictsMode.TakeLocalVersionForAll)
//                        || (options.Mode == TFSResolveConflictsMode.AnalizeCustomPaths
//                            && (options.ServerPathsTakeLocalVersion.Any(s => (conflict.ServerPath).StartsWith(s))
//                                || (options.LocalPathsTakeLocalVersion.Any(s => (conflict.LocalPath).StartsWith(s))))))
//                    {
//                        // оставляем локальный файл, затирая на сервере
//                        conflict.Resolution = Resolution.AcceptYours;
//                        resolveAsLocalConflicts.Add(conflict);
//                    }
//                    else
//                    {
//                        // конфликт для неизвестного файла
//                        unresolvedConflicts.Add(conflict);
//                    }
//                }

//                Conflict[] unresolvedConflicts2 = null;

//                if (resolveAsServerConflicts.Count > 0)
//                {
//                    _ws.ResolveConflicts(resolveAsServerConflicts, null, out unresolvedConflicts2);
//                    // не все конфликты удалось разрешить
//                    if (unresolvedConflicts2.Length > 0)
//                    {
//                        foreach (Conflict conflict in unresolvedConflicts2)
//                        {
//                            unresolvedFilesList.Add(GetTFSFileInfoFromConflict(conflict));
//                        }
//                    }
//                }

//                if (resolveAsLocalConflicts.Count > 0)
//                {
//                    _ws.ResolveConflicts(resolveAsLocalConflicts, null, out unresolvedConflicts2);
//                    // не все конфликты удалось разрешить
//                    if (unresolvedConflicts2.Length > 0)
//                    {
//                        foreach (Conflict conflict in unresolvedConflicts2)
//                        {
//                            unresolvedFilesList.Add(GetTFSFileInfoFromConflict(conflict));
//                        }
//                    }
//                }

//                if (unresolvedConflicts.Count > 0)
//                {
//                    foreach (Conflict conflict in unresolvedConflicts)
//                    {
//                        unresolvedFilesList.Add(GetTFSFileInfoFromConflict(conflict));
//                    }
//                }

//                result = new TFSResolveResult()
//                {
//                    Success = true,
//                    NumResolvedConflicts = conflicts.Length - unresolvedFilesList.Count,
//                    UnresolvedFiles = unresolvedFilesList.ToArray()
//                };
//            }
//            catch (Exception ex)
//            {
//                result = new TFSResolveResult()
//                {
//                    Success = false,
//                    ErrorMessage = ex.Message
//                };
//            }

//            return result;
//        }
//        private static ItemSpec[] CreateItemSpecArray(string[] projects)
//        {
//            int count = projects.Length;
//            ItemSpec[] items = new ItemSpec[count];
//            for (int index = 0; index < count; index++) {
//                items[index] = new ItemSpec(projects[index], RecursionType.Full);
//            }
//            return items;
//        }
//        /// <summary>
//        /// Вернуть изменения на сервер из указаных папок
//        /// </summary>
//        /// <param name="projectPaths">Пути к папкам на сервере, которые будут возвращены на сервер</param>
//        /// <param name="work_item_ids">Номера рабочих элементов (work items), которые будут ассоциированы с изменениями</param>
//        /// <param name="comment">Комментарий при возврате</param>
//        /// <returns>Вернет true, если check in прошел успешно</returns>
//        public TFSCheckInResult CheckIn(string[] projectPaths, int[] work_item_ids, string comment)
//        {
//            ItemSpec[] items = CreateItemSpecArray(projectPaths);
//            PendingChange[] changes = _ws.GetPendingChanges(items);
//            var result = new TFSCheckInResult();
//            if (changes.Length > 0) {
//                result.HasChanges = true;
//                try {
//                    var options = new WorkspaceCheckInParameters(changes, comment);
//                    options.QueueBuildForGatedCheckIn = true;
//                    if (!Array.IsNullOrEmpty(work_item_ids)) {
//                        WorkItemCheckinInfo[] wi = new WorkItemCheckinInfo[work_item_ids.Length];
//                        for (int index = 0; index < work_item_ids.Length; index++) {
//                            wi[index] = new WorkItemCheckinInfo(this._wistore.GetWorkItem(work_item_ids[index]), WorkItemCheckinAction.Associate);
//                        }
//                        options.AssociatedWorkItems = wi;
//                    }
//                    result.ChangesetId = _ws.CheckIn(options);
//                } catch (GatedCheckinException ex) {
//                    result.GatedBuild = true;
//                    result.GatedBuildName = ex.ShelvesetName;
//                } catch (Exception ex) {
//                    result.Success = false;
//                    result.ErrorMessage = ex.Message;
//                    return result;
//                }
//            }
//            result.Success = true;
//            return result;
//        }
//        /// <summary>
//        /// Запустить билд по имени
//        /// </summary>
//        /// <param name="buildName">Путь к описанию билда. Например, "root\3.9.8.0.infoenergo.all"</param>
//        /// <returns>Вернет true, если билд успешно запущен</returns>
//        public bool QueueBuild(string buildName)
//        {
//            try {
//                IBuildServer bs = _tfs.GetService<IBuildServer>();
//                IBuildDefinition buildDefinition = bs.GetBuildDefinition(string.Empty, buildName);
//                IBuildRequest buildRequest = buildDefinition.CreateBuildRequest();
//                bs.QueueBuild(buildRequest);
//            } catch (Exception) {
//                return false;
//            }
//            return true;
//        }
//        public bool UndoChanges(string[] projectPaths)
//        {
//            //ItemSpec[] items = projectPaths.Select(p => new ItemSpec(p, RecursionType.Full)).ToArray();
//            ItemSpec[] items = CreateItemSpecArray(projectPaths);
//            try {
//                int changesCount = this._ws.Undo(items);
//            } catch (Exception) {
//                return false;
//            }
//            return true;
//        }
//        public TFSGetResult Merge(Tuple<string, string>[] mergePaths)
//        {
//            TFSGetResult result = new TFSGetResult();
//            try
//            {
//                foreach (Tuple<string, string> mergePath in mergePaths)
//                {
//                    var getStatus = _ws.Merge(mergePath.Item1, mergePath.Item2, null, null, LockLevel.None, RecursionType.Full, MergeOptions.None);
//                    result.NumConflicts += getStatus.NumConflicts;
//                    result.NumUpdated += getStatus.NumUpdated;

//                    if (getStatus.NumFailures > 0)
//                    {
//                        // некоторые ошибки можно смело игнорировать
//                        var criticalFailures = getStatus.GetFailures().Where(f => f.Code != "MergeEditDeleteException").ToList();
//                        if (criticalFailures.Count != 0) {
//                            result.NumFailures += criticalFailures.Count;
//                            result.FailuresMessage = result.FailuresMessage + string.Join("\r\n", criticalFailures.SelectAsArray(f => f.Message));
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                result.Success = false;
//                result.ErrorMessage = ex.Message;
//                return result;
//            }

//            result.Success = true;
//            return result;
//        }

//        public bool AddFile(string filePath)
//        {
//            int addedCount = 0;

//            try
//            {
//                addedCount = _ws.PendAdd(filePath);
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }

//            return true;
//        }

//        // медленно
//        public bool CheckOutFile(string[] filePaths)
//        {
//            int checkedOutCount = 0;

//            try
//            {
//                checkedOutCount = _ws.PendEdit(filePaths);
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }

//            return true;
//        }

//        private TFSFileInfo GetTFSFileInfoFromConflict(Conflict conflict)
//        {
//            return new TFSFileInfo() { ServerPath = conflict.ServerPath, ErrorText = conflict.GetBriefMessage() };
//        }

//        #region IDisposable
//        public void Dispose()
//        {
//            if (_tfs != null && !_tfs.Disposed)
//            {
//                _tfs.Dispose();
//            }
//        }
//        #endregion
//    }

//    internal class TFSResolveConflictsOptions
//    {
//        public TFSResolveConflictsMode Mode { get; set; }

//        public List<string> LocalPathsTakeLocalVersion;
//        public List<string> ServerPathsTakeLocalVersion;

//        public List<string> LocalPathsTakeServerVersion;
//        public List<string> ServerPathsTakeServerVersion;

//        public TFSResolveConflictsOptions()
//        {
//            LocalPathsTakeLocalVersion = new List<string>();
//            ServerPathsTakeLocalVersion = new List<string>();
//            LocalPathsTakeServerVersion = new List<string>();
//            ServerPathsTakeServerVersion = new List<string>();
//        }

//        public static TFSResolveConflictsOptions Default
//        {
//            get { return new TFSResolveConflictsOptions() {Mode = TFSResolveConflictsMode.None};}
//        }
//    }

//    internal enum TFSResolveConflictsMode
//    {
//        /// <summary>
//        /// Ничего не делать с конфликтами
//        /// </summary>
//        None,
//        /// <summary>
//        /// В случае конфликта всегда брать локальную версию
//        /// </summary>
//        TakeLocalVersionForAll,
//        /// <summary>
//        /// В случае конфликта всегда брать версию с сервера
//        /// </summary>
//        TakeServerVersionForAll,
//        /// <summary>
//        /// Принимать решение в зависимости от настроек, переданных в Options
//        /// </summary>
//        AnalizeCustomPaths
//    }
//}