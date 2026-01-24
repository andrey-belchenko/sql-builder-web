using System;
using Contract = System.Diagnostics.Contracts.Contract;
using System.Collections.Generic;
using System.IO;
using System.Linq;
////using System.Windows.Forms;
using System.Xml.Linq;

using sql.builder.DataApi;
using sql.builder.XmlHelpers;

namespace sql.builder.Core
{
    internal class ProjectManager
    {
        // метаданные всех проектов (в том числе незагруженных)
        private Dictionary<string, Project> _projects;
        private VSXElement _schemeOld;
        private ProjectsController _controller;
        internal ProjectManager()
        {
            this.ReloadProjects();
        }
        internal void ReloadProjects()
        {
            this._projects = new Dictionary<string, Project>();
            // чтобы подтянуть изменения напрямую из файлов
            foreach (XElement xproject in XmlSpecialFiles.GetActualXml().Elements(EName.projects).Elements(EName.project)) {
                Project project = new Project(xproject);
                if (!this._projects.ContainsKey(project.Name)) {
                    this._projects.Add(project.Name, project);
                }
                if (XmlReports.GetDemandProjects().Contains(project.Name)) {
                    project.LoadIfNeed();
                }
            }
            if (_controller != null) {
                _controller.ReloadProjects();
            }
        }
        internal void LoadProjectIfNeed(string name)
        {
            Project proj = this._projects[name];
            if (proj.Hidden) {
                proj.Show();
            }
            if (!proj.Loaded) {
                proj.LoadIfNeed();
                // подгружаем зависимости
                this.LoadProjectsIfNeed(proj.ReferencesNames);
            }
        }
        internal void LoadProjectsIfNeed(IList<string> names)
        {
            for (int index = 0; index < names.Count; index++) {
                this.LoadProjectIfNeed(names[index]);
            }
        }
        /// <summary>
        /// Нужно при перезагрузке схемы
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal void HideProject(string name)
        {
            this._projects[name].Hide();
        }
        internal void SetProjectScheme(string name, VSXElement scheme, VSXElement native_scheme)
        {
            Project project;
            if (!this._projects.TryGetValue(name, out project)) {
                project = new Project(name);
                this._projects.Add(project.Name, project);
            }
            project.SetProjectScheme(scheme, native_scheme);
        }
        /*internal void SetProjectScheme(string name, VSXElement scheme)
        {
            Project project;
            if (!_projects.TryGetValue(name, out project)) {
                project = new Project(name);
                _projects.Add(project.Name, project);
            }
            project.SetProjectScheme(scheme);
        }
        internal void SetProjectNativeScheme(string name, VSXElement schemeNative)
        {
            Project project;
            if (!_projects.TryGetValue(name, out project)) {
                project = new Project(name);
                _projects.Add(project.Name, project);
            }
            project.SetProjectNativeScheme(schemeNative);
        }*/
        internal IEnumerable<Project> GetVisibleProjects()
        {
            return this._projects.Values.Where(Project.IsVisible);
        }
        /*internal IEnumerable<Project> GetHiddenProjects()
        {
            return _projects.Values.Where(p => p.Loaded && p.Hidden);
        }*/
        internal IEnumerable<Project> GetLoadedProjects()
        {
            return this._projects.Values.Where(Project.IsLoaded);
        }
        internal IEnumerable<Project> GetAllProjects()
        {
            return this._projects.Values;
        }
        internal Project GetProject(string project_name)
        {
            return this._projects[project_name];
        }
        private Stack< bool> _oldOnly = new Stack<bool>();
        internal void PushOldOnly(bool value)
        {
            this._oldOnly.Push(value);
        }
        internal void PopOldOnly()
        {
            this._oldOnly.Pop();
        }
        internal bool IsOldOnly()
        {
            if (this._oldOnly.Count == 0) {
                return false;
            } else {
                return this._oldOnly.Peek();
            }
        }
        internal IList<VSXElement> GetScheme()
        {
            if (this.IsOldOnly()) {
                return this.GetOldScheme();
            } else {
                var list = new List<VSXElement>();
                foreach (Project project in this._projects.Values) {
                    if (!project.Hidden) {
                        VSXElement scheme = project.Scheme;
                        if (scheme != null) {
                            list.Add(scheme);
                        }
                    }
                }
                return list;
            }
        }
        internal IList<VSXElement> GetOldScheme()
        {
            if (this._schemeOld != null) {
                return new VSXElement[1] { this._schemeOld };
            } else {
                //string name;
                /*if (_projects.Any(p => p.Value.Loaded && p.Value.Name == "asuse2")) {
                    name = "asuse2";
                } else if (_projects.Any(p => p.Value.Loaded && p.Value.Name == "asuse1")) {
                    name = "asuse1";
                } else {
                    return Enumerable.Empty<VSXElement>(); // "Не удалось загрузить старую схему;
                }*/
                string name = "asuse2";
                Project project;
                if (!_projects.TryGetValue(name, out project) || !project.Loaded) {
                    name = "asuse1";
                    if (!_projects.TryGetValue(name, out project) || !project.Loaded) {
                        return Array.Empty<VSXElement>(); // "Не удалось загрузить старую схему;
                    }
                }
                string path = Path.Combine(XmlReports.GetCurrentContentFolder(), name + ".old.xml");
                this._schemeOld = VSXElement.Get(XElement.Load(path));
                VSXElement[] arr = new VSXElement[1] { this._schemeOld };
                Compiler.forCustomersProcessing(arr, XmlReports.customerId);
                return arr;
            }
        }
        internal IList<VSXElement> GetNativeScheme()
        {
            var list = new List<VSXElement>();
            foreach (Project project in this._projects.Values) {
                if (!project.Hidden) {
                    VSXElement native_scheme = project.SchemeNative;
                    if (native_scheme != null) {
                        list.Add(native_scheme);
                    }
                }
            }
            return list;
        }
        internal ProjectsController GetController()
        {
            if (this._controller == null) {
                this._controller = new ProjectsController();
            }
            this._controller.UpdateStatus();
            return this._controller;
        }
    }
    internal class Project
    {
        #region static
        internal static bool IsVisible(Project p)
        {
            return !p.hidden;
        }
        internal static bool IsLoaded(Project p)
        {
            return p.loaded;
        }
        #endregion
        #region поля
        private string name;
        private bool loaded;
        private VSXElement scheme;
        private VSXElement scheme_native;
        private bool hidden;
        private string project_path;
        private string runtime_path;
        private List<string> references_names;
        private List<string> masters_names;
        #endregion
        /// <summary>
        /// Имя проекта
        /// </summary>
        internal string Name { get { return this.name; } }
        /// <summary>
        /// Путь до папки проекта: \sql.builder\projects\&lt;имя проекта&gt;\
        /// </summary>
        internal string ProjectPath { get { return this.project_path; } }
        /// <summary>
        /// Схема из файла проекта \sql.builder\projects\&lt;имя проекта&gt;\&lt;имя проекта&gt;.xml
        /// </summary>
        internal VSXElement Scheme { get { return this.scheme; } }
        /// <summary>
        /// Схема из native-файла проекта \sql.builder\projects\&lt;имя проекта&gt;\&lt;имя проекта&gt;.native.xml
        /// </summary>
        internal VSXElement SchemeNative { get { return this.scheme_native; } }
        /// <summary>
        /// Имена проектов, зависимых от этого
        /// </summary>
        internal IList<string> MastersNames { get { return this.masters_names; } }
        /// <summary>
        /// Имена проектов, от которых зависит этот
        /// </summary>
        internal IList<string> ReferencesNames { get { return this.references_names; } }
        internal bool Loaded { get { return this.loaded; } }
        internal bool Hidden { get { return this.hidden; } }
        /// <summary>
        /// Имя native-файла проекта \sql.builder\projects\&lt;имя проекта&gt;\&lt;имя проекта&gt;.native.xml
        /// </summary>
        internal string FileNativePath {
            get {
                return Path.Combine(this.project_path, this.name + ".native.xml");
            }
        }
        /// <summary>
        /// Имя файла проекта \sql.builder\projects\&lt;имя проекта&gt;\&lt;имя проекта&gt;.xml
        /// </summary>
        internal string FileCompiledPath
        {
            get {
                return Path.Combine(this.project_path, this.name + ".xml");
            }
        }
        internal Project(string name)
        {
            this.name = name;
        }
        internal Project(XElement xproject)
        {
            Contract.Assume(xproject != null);
            Contract.Assume(xproject.Name == EName.project);
            this.name = xproject.Attribute(AName.name).Value;
            //
            this.references_names = new List<string>();
            foreach (XElement reference in xproject.Elements(EName.references).Elements(EName.reference)) {
                this.references_names.Add(reference.Attribute(AName.project).Value);
            }
            // Определяем зависимые проекты
            this.masters_names = new List<string>();
            foreach (XElement el in xproject.Parent.Elements(EName.project)) {
                string name = el.Attribute(AName.name).Value;
                if (name != this.name) {
                    foreach (XElement reference in el.Elements(EName.references).Elements(EName.reference)) {
                        if (reference.Attribute(AName.project).Value == this.name) {
                            this.masters_names.Add(name);
                            break;
                        }
                    }
                }
            }
            XAttribute attr = xproject.Attribute(AName.directory);
            if (attr == null) {
                this.project_path = Path.Combine(XmlReports.GetDefaultSourceFolder(), this.name);
                this.runtime_path = Path.Combine(XmlReports.GetRuntimePath(), "sql.builder", XmlReports.SourceFolderName, this.name);
            } else {
                this.project_path = Path.Combine(XmlReports.GetRootPath(), attr.Value, this.name);
                this.runtime_path = Path.Combine(XmlReports.GetRuntimePath(), attr.Value);
            }
        }
        internal void LoadIfNeed()
        {
            if (!this.loaded) {
                string rootPath = XmlReports.UseProjectSourceFolder ? this.project_path : this.runtime_path;
                this.scheme = VSXElement.Get(XElement.Load(Path.Combine(rootPath, this.name + ".xml")));
                Compiler.forCustomersProcessing(new[] { this.scheme }, XmlReports.customerId);
                this.scheme_native = VSXElement.Get(XElement.Load(Path.Combine(rootPath, this.name + ".native.xml")));
                if (this.name != "common") {
                    foreach (XElement xpar in this.scheme.Elements(EName.globalparams).Elements(EName.param)) {
                        string parname = xpar.Attribute(AName.name).Value;
                        if (!XmlReports.IsGlobalParExists(parname)) {
                            XmlReports.SetGlobalParValue(parname, xpar.Value);
                        }
                    }
                }
                this.loaded = true;
                VCashUtils.ClearCash();
            }
        }
        internal void SetProjectScheme(VSXElement scheme, VSXElement native_scheme)
        {
            Contract.Assume(scheme != null);
            Contract.Assume(native_scheme != null);
            this.scheme = scheme;
            this.scheme_native = native_scheme;
            this.loaded = true;
        }
        /*internal void SetProjectScheme(VSXElement scheme)
        {
            this.scheme = scheme;
            this.loaded = true;
        }
        internal void SetProjectNativeScheme(VSXElement schemeNative)
        {
            this.scheme_native = schemeNative;
            this.loaded = true;
        }*/
        internal void Unload()
        {
            if (this.loaded) {
                this.scheme = null;
                this.scheme_native = null;
                this.loaded = false;
            }
        }
        internal void Hide()
        {
            this.hidden = true;
        }
        internal void Show()
        {
            this.hidden = false;
        }
    }
    /// <summary>
    /// Для работы с UI
    /// </summary>
    internal class ProjectsController
    {
        internal Dictionary<string, ProjectRecord> Projects { get; private set; }

        internal ProjectsController()
        {
            ReloadProjects();          
        }

        internal void UncheckAll()
        {
            foreach (ProjectRecord projectRecord in Projects.Values) {
                if (projectRecord.Name != "common" && projectRecord.Checked) {
                    projectRecord.StatusChanging -= RecordOnStatusChanging;
                    projectRecord.Checked = false;
                    projectRecord.StatusChanging += RecordOnStatusChanging;
                }
            }
        }
        /*internal void CheckList(object[] list)
        {
            foreach (ProjectRecord projectRecord in list)
            {
               // projectRecord.StatusChanging -= RecordOnStatusChanging;
                projectRecord.Checked = true;
               // projectRecord.StatusChanging += RecordOnStatusChanging;
            }
        }*/
        internal void ReloadProjects()
        {
            Projects = new Dictionary<string, ProjectRecord>();
            var projects = XmlReports.Environment.Manager.GetAllProjects();
            foreach (Project project in projects)
            {
                var rec = new ProjectRecord(project);
                Projects.Add(rec.Name, rec);

                rec.StatusChanging += RecordOnStatusChanging;
                rec.StatusChanged += RecordOnStatusChanged;
            }
        }

        internal void UpdateStatus()
        {
            foreach (var p in Projects.Values) p.UpdateStatus();
        }

        internal void AcceptChanges()
        {
            var projects_to_load = Projects.Values.Where(p => p.Status == ProjectStatus.ReadyToLoad).ToList();
            var projects_to_unload = Projects.Values.Where(p => p.Status == ProjectStatus.ReadyToUnload).ToList();
            if (projects_to_load.Count == 0 && projects_to_unload.Count == 0) return;
            foreach (var project in projects_to_load) {
                project.Project.LoadIfNeed();
                project.UpdateStatus();
            }
            foreach (var project in projects_to_unload) {
                project.Project.Unload();
                project.UpdateStatus();
            }
            var args = new ProjectsStateChangedArgs(projects_to_load.SelectAsArray(ProjectRecord.GetName), projects_to_unload.SelectAsArray(ProjectRecord.GetName));
            ProjectsStateChanged(this, args);
        }

        internal void SaveState()
        {
            var xprojects = new XElement(EName.projects);
            foreach (ProjectRecord record in Projects.Values) {
                record.DefaultLoaded = (record.Status == ProjectStatus.Loaded);
                if (record.Status == ProjectStatus.Loaded) {
                    xprojects.Add(new XElement(EName.project, new XAttribute(AName.name, record.Name)));
                }
            }
            SettingsHelper.ProjectsState = xprojects.ToString();
        }

        internal void LoadState()
        {
            string state = SettingsHelper.ProjectsState;
            if (state == null) return;

            var xprojects = XElement.Parse(state);
            foreach (var xproject in xprojects.Elements(TextConst.EName.Project))
            {
                ProjectRecord rec = null;
                if(!Projects.TryGetValue(xproject.Attribute(TextConst.AName.Name).Value, out rec))
                {
                    continue;
                }

                rec.DefaultLoaded = true;
                rec.Project.LoadIfNeed();
                rec.UpdateStatus();
            }
        }

        private void RecordOnStatusChanging(object sender, StatusChangingArgs args)
        {
            var rec = (ProjectRecord)sender;
            // уже загружен
            //if (rec.Status == ProjectStatus.Loaded)
            //{
            //    args.Cancel = true;
            //}
            // один из загружающих уже загружен или будет загружен
            if (args.NewValue == ProjectStatus.NotLoaded || args.NewValue == ProjectStatus.ReadyToUnload) {
                IList<ProjectRecord> loadedMasters = rec.Project.MastersNames
                    .SelectAsArray(pn => Projects[pn])
                    .Where(p => p.Status == ProjectStatus.Loaded || p.Status == ProjectStatus.ReadyToLoad)
                    .ToList();
                if (loadedMasters.Count != 0) {
                    string message = string.Format("Невозможно выгрузить {0}, т.к. на него ссылаются другие загруженные проекты {1}", 
                        rec.Name, string.Join(", ", loadedMasters.SelectAsArray(ProjectRecord.GetName)));
                    HasMessage(this, new HasMessageArgs(message));
                    args.Cancel = true;
                }
            }
        }
        private void RecordOnStatusChanged(object sender, StatusChangedArgs args)
        {
            var rec = (ProjectRecord)sender;

            if (args.Status == ProjectStatus.ReadyToLoad)
            {
                // все незагруженные референсы тоже подгрузятся
                var references = rec.Project.ReferencesNames.SelectAsArray(pn => Projects[pn]);

                foreach (var reference in references.Where(p => p.Status == ProjectStatus.NotLoaded))
                {
                    reference.Status = ProjectStatus.ReadyToLoad;
                }

                foreach (var reference in references.Where(p => p.Status == ProjectStatus.ReadyToUnload))
                {
                    reference.Status = ProjectStatus.Loaded;
                }
            }
            //else if (args.NewValue == ProjectStatus.NotLoaded)
            //{
            //    // все кто ссылаются на проект не загружаются если не осталось загружаемых референсов
            //    var references = rec.Project.ReferencesNames.Select(pn => Projects[pn]).Where(p => p.Status == ProjectStatus.ReadyToLoad);
            //    foreach (var reference in references)
            //    {
            //        bool has_loaded_masters = rec.Project.MastersNames.Select(pn => Projects[pn]).Any(p => p.Status == ProjectStatus.ReadyToLoad || p.Status == ProjectStatus.Loaded);
            //        if (!has_loaded_masters) reference.Status = ProjectStatus.NotLoaded;
            //    }
            //}
        }

        internal event EventHandler<ProjectsStateChangedArgs> ProjectsStateChanged = delegate { };
        internal event EventHandler<HasMessageArgs> HasMessage = delegate { };
    }
    // Используется в \root\main\all\sql.builder\Controls\ucProjects.cs
    // Свойства должны быть public, иначе Data Binding не сможет их прочитать
    internal class ProjectRecord
    {
        private Project project;
        private bool default_loaded;
        private ProjectStatus _status;
        internal Project Project { get { return this.project; } }
        public bool Checked {
            get {
                return this._status == ProjectStatus.ReadyToLoad || this._status == ProjectStatus.Loaded;
            }
            set {
                if (this.project.Loaded) {
                    this.Status = (value) ? ProjectStatus.Loaded : ProjectStatus.ReadyToUnload;
                } else {
                    this.Status = (value) ? ProjectStatus.ReadyToLoad : ProjectStatus.NotLoaded;
                }
            }
        }
        public string Name { get { return this.project.Name; } }
        public string References { get { return string.Join(", ", this.project.ReferencesNames); } }
        public string Masters { get { return string.Join(", ", this.project.MastersNames); } }
        public bool DefaultLoaded { get { return this.default_loaded; } set { this.default_loaded = value; } }
        public ProjectStatus Status
        {
            get {
                return this._status;
            }
            set {
                if (this._status == value) {
                    return;
                }
                if (this.StatusChanging != null) {
                    var args = new StatusChangingArgs(this._status, value);
                    this.StatusChanging(this, args);
                    if (args.Cancel) return;
                }
                this._status = value;
                if (this.StatusChanged != null) {
                    this.StatusChanged(this, new StatusChangedArgs(this._status));
                }
            }
        }
        internal ProjectRecord(Project project)
        {
            this.project = project;
            this.UpdateStatus();
        }
        internal void UpdateStatus()
        {
            this._status = (this.project.Loaded) ? ProjectStatus.Loaded : ProjectStatus.NotLoaded;
        }
        internal event EventHandler<StatusChangingArgs> StatusChanging;
        internal event EventHandler<StatusChangedArgs> StatusChanged;
        internal static string GetName(ProjectRecord pr)
        {
            return pr.Name;
        }
    }
    internal enum ProjectStatus
    {
        NotLoaded = 0,
        ReadyToLoad = 1,
        Loaded = 2,
        ReadyToUnload = 3
    }
    internal class StatusChangingArgs : EventArgs
    {
        private ProjectStatus old_value, new_value;
        private bool cancel;
        internal ProjectStatus OldValue { get { return this.old_value; } }
        internal ProjectStatus NewValue { get { return this.new_value; } }
        internal bool Cancel { get { return this.cancel; } set { this.cancel = value; } }
        internal StatusChangingArgs(ProjectStatus oldValue, ProjectStatus newValue)
        {
            this.old_value = oldValue;
            this.new_value = newValue;
            this.cancel = false;
        }
    }
    internal class StatusChangedArgs : EventArgs
    {
        private ProjectStatus status;
        internal ProjectStatus Status { get { return this.status; } }
        internal StatusChangedArgs(ProjectStatus status)
        {
            this.status = status;
        }
    }
    internal class ProjectsStateChangedArgs : EventArgs
    {
        internal string[] LoadedProjects { get; private set; }
        internal string[] UnloadedProjects { get; private set; }

        public ProjectsStateChangedArgs(string[] projects_to_load, string[] projects_to_unload)
        {
            LoadedProjects = projects_to_load;
            UnloadedProjects = projects_to_unload;
        }
    }

    internal class HasMessageArgs : EventArgs
    {
        internal string Message {get; private set;}

        public HasMessageArgs(string message)
        {
            Message = message;
        }
    }
}